# Products API

## Docker

The API and PostgreSQL run with Docker Compose. Set a generated password in a local `.env` file before starting. `.env` is ignored by Git and excluded from Docker builds; `.env.example` documents the variable. A local `.env` may already have been generated during setup.

```sh
# Only when .env does not already exist:
cp .env.example .env
# Replace the placeholder password in .env, then:
docker compose up --build -d
```

The API is available at `http://localhost:8080/api/v1/products`. PostgreSQL is accessible only on the Compose network and stores data in the `postgres_data` volume. Compose enables `Database__MigrateOnStartup=true`; the application default is false.

Changing `.env` does not change an existing PostgreSQL volume's password. Existing databases require a separate password rotation. The former password committed in application settings should be considered exposed and replaced wherever it is still in use.

## Run outside Docker

Supply `ConnectionStrings__Postgres` through the environment or a local secret provider; the repository does not contain database credentials. For example, in your shell set it to a connection string with your own host, database, user, and password before running:

```sh
# Enable only if this instance should apply migrations:
export Database__MigrateOnStartup=true
dotnet run --project src/ProductsService.Api
```

## Request limits

- Prices must be positive, at most `9999999999999999.99`, with at most two significant decimal places. Trailing fractional zeroes are accepted.
- Stock is stored as a non-negative 32-bit integer. An increment exceeding `2147483647` returns 400 without changing the product.
- Paging defaults to page 1 and 10 items; page sizes range from 1 to 50. Offsets beyond `2147483647` return a validation error.
- Successful detail updates set `UpdatedAt` in UTC and preserve `CreatedAt`.

## Tests

Docker must be running for the PostgreSQL Testcontainers tests.

```sh
dotnet test ProductsService.slnx
```

See [tests/README.md](tests/README.md) for the test setup.
