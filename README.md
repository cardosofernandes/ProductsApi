# Products API

Product and stock management as an **ASP.NET Core Minimal API** on **.NET 10 / EF Core 10 / PostgreSQL 17**,
organised into API, Business and Data layers. It uses FluentValidation, Mapster, versioned OpenAPI docs,
Scalar and Problem Details error responses, with Serilog for request logging.

## Run it

```bash
# Only if .env does not already exist:
cp .env.example .env              # replace POSTGRES_PASSWORD with a generated password
docker compose up --build -d
```

| What | URL / connection |
|---|---|
| Products API (Docker) | http://localhost:8080/api/v1/products |
| Products API (local HTTP profile) | http://localhost:5289/api/v1/products |
| Scalar API reference (Development) | http://localhost:5289/scalar/v1 |
| OpenAPI JSON (Development) | http://localhost:5289/openapi/v1.json |
| PostgreSQL (Compose network only) | `postgres:5432`, db `products`, user `products` |

Compose enables `Database__MigrateOnStartup=true`; migrations run before the API starts. The application
default is `false`. PostgreSQL stores data in the `postgres_data` volume and has no published host port.
Documentation routes are mapped only in Development and are unavailable in the default Compose environment.

`.env` is ignored by Git and excluded from Docker builds. Changing its password does not update an existing
PostgreSQL volume; rotate that database's password separately. Replace the former password committed in
application settings wherever it is still in use.

**Without Docker for the API:** use a reachable PostgreSQL instance and supply your connection string through
the environment or a local secret provider. The repository contains no database credentials.

```bash
export ConnectionStrings__Postgres='Host=localhost;Port=5432;Database=products;Username=products;Password=<your-password>'
export Database__MigrateOnStartup=true   # only when this instance should apply migrations
dotnet run --project src/ProductsService.Api --launch-profile http
```

**Tests:** `dotnet test ProductsService.slnx`. Integration tests need Docker running; Testcontainers starts a
throw-away PostgreSQL 17 database and applies migrations. Unit tests use mocked repositories and need no database.

## Layout

Three application projects; project references follow Api → Business → Data.

```text
src/
  ProductsService.Api                 Minimal API host
    Endpoints/                        product CRUD, search, stock filtering and stock adjustments
    Filters/                          FluentValidation endpoint filter
    Errors/                           global exception → Problem Details mapping
    Startup/                          API versioning, OpenAPI, error handling and database initialization
  ProductsService.Business            services, validation and DTO mapping; depends on Data
    DTO/                              create/update requests and product responses
    Interfaces/ Services/             IProductService, ProductService
    Validators/ Mapping/              FluentValidation rules, Mapster configuration
    Exceptions/                       missing-product and invalid-stock exceptions
  ProductsService.Data                EF Core and Npgsql persistence
    Entities/                         Product, Category
    Interfaces/ Repositories/         IProductRepository, ProductRepository
    Common/                           paging models and query extensions
    Configurations/ Migrations/       entity constraints, JSON seeding and schema migrations
    SeedData/                         product and category JSON
tests/
  ProductsService.UnitTests           xUnit + Moq + Shouldly service tests
  ProductsService.IntegrationTests    WebApplicationFactory + PostgreSQL Testcontainers
```

**Request flow:** endpoint validates the request through a FluentValidation filter → `IProductService`
applies business logic and maps DTOs with Mapster → `IProductRepository` queries or updates PostgreSQL through
`AppDbContext`. Application exceptions are mapped to Problem Details by the global exception handler.

**Writes** create, update or delete products and commit with `SaveChangesAsync`. Detail updates preserve
`CreatedAt` and set `UpdatedAt` in UTC. Stock adjustments use an atomic database update and also set `UpdatedAt`.
**Reads** use no-tracking queries, include the category, and map the loaded entities to response DTOs.

## Endpoints (`/api/v1/products`)

| Method | Route | Result |
|---|---|---|
| GET | `/` | Paged products |
| GET | `/{productId}` | Product details |
| GET | `/search?name=...` | Paged, case-insensitive name substring search |
| GET | `/stock-level?minStock=0&maxStock=100` | Paged products in an inclusive stock range |
| POST | `/` | Create product; 201 with a Location header |
| PUT | `/{productId}` | Update product details; 204 |
| DELETE | `/{productId}` | Delete product; 204 |
| POST | `/{productId}/add-to-stock/{quantity}` | Increment stock; 200 |
| POST | `/{productId}/decrement-stock/{quantity}` | Decrement stock; 200 |

List, search and stock-range endpoints accept `pageNumber` (default 1) and `pageSize` (default 10, maximum 50).
Results are ordered by product ID and include `items`, `pageNumber`, `pageSize`, `totalCount`, `totalPages`,
`hasPrevious` and `hasNext`. Page offsets greater than `2147483647` fail validation.

Create requests contain `name`, `description`, `price`, `stock` and `categoryId`. Update requests contain the
same fields except `stock`; use the dedicated stock endpoints to adjust quantities. Product responses include
the category name; creation responses include `categoryId`.

Names are required and limited to 64 characters; descriptions are optional and limited to 256. Prices must be
positive, at most `9999999999999999.99`, with at most two significant decimal places (trailing fractional zeroes
are accepted). Stock must be a non-negative 32-bit integer. Adjustment quantities must be positive; decrements
below zero and increments above `2147483647` return 400 without changing the product.

## Authentication

All endpoints are public. Authentication and role-based authorization are not configured.

## Error handling

Problem Details responses use `application/problem+json`. The shared customization adds `traceId`, a UTC
`timestamp`, and an `instance` containing the request method and path.

| Source | Status | How |
|---|---|---|
| Request validation | 400 + `errors` | `ValidationFilter<T>` returns `Results.ValidationProblem` |
| Missing product | 404 | `ProductNotFoundException` mapped by `GlobalExceptionHandler` |
| Invalid stock quantity or overflow | 400 | Application/data exceptions mapped by `GlobalExceptionHandler` |
| Optimistic concurrency, unique or foreign-key violation | 409 | EF Core / PostgreSQL exception mapping |
| Database check constraint violation | 400 | PostgreSQL exception mapping |
| Empty error responses, including unknown routes | Matching status | `UseStatusCodePages()` + `AddProblemDetails()` |
| Anything else | 500 | Generic error detail; exception logged by `GlobalExceptionHandler` |

## Data

EF Core uses snake_case names, including `products` and `categories`. Migrations seed **50 products and
3 categories** through `HasData` from JSON files copied to the application output directory.

Product names are unique, prices use `numeric(18,2)`, and database constraints enforce positive prices,
non-negative stock and six-digit product IDs. Generated product IDs start at `100000` and end at `999999`.
`Stock` is an optimistic-concurrency token. Category deletion is restricted while products reference it.

```bash
# With ConnectionStrings__Postgres configured and the dotnet-ef tool installed:
dotnet ef migrations add <Name> -p src/ProductsService.Data -s src/ProductsService.Api -o Migrations
```
