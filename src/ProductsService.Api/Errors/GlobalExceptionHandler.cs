using ProductsService.Data.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ProductsService.Business.Exceptions;

namespace ProductsService.Api.Errors;

internal sealed class GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = MapException(exception is DbUpdateException { InnerException: PostgresException pg } ? pg : exception);
        
        if(statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Unhandled exception processing {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);
        else
            logger.LogInformation("Request {Method} {Path} failed with {Status}: {Reason}", httpContext.Request.Method, httpContext.Request.Path, statusCode, exception.Message);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails { Status = statusCode, Title = title, Detail = detail };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }

    private static (int Status, string Title, string Detail) MapException(Exception ex) => ex switch
    {
        ProductNotFoundException appEx => (StatusCodes.Status404NotFound, "Product not found", appEx.Message),
        
        StockOverflowException => (StatusCodes.Status400BadRequest, "Invalid stock quantity", "The resulting stock exceeds the supported range."),

        StockQuantityOutOfRangeException => (StatusCodes.Status400BadRequest, "Invalid stock quantity", "The stock quantity is out of range."),
        
        DbUpdateConcurrencyException => (StatusCodes.Status409Conflict, "Concurrency conflict", "The resource was modified by another request. Reload it and try again."),
        
        PostgresException { SqlState: PostgresErrorCodes.UniqueViolation } pg =>
            (StatusCodes.Status409Conflict, "Duplicate resource", $"A resource with the same key already exists ({pg.ConstraintName})."),

        PostgresException { SqlState: PostgresErrorCodes.ForeignKeyViolation } pg =>
            (StatusCodes.Status409Conflict, "Related resource missing", $"The operation violates a relationship constraint ({pg.ConstraintName})."),

        PostgresException { SqlState: PostgresErrorCodes.CheckViolation } pg =>
            (StatusCodes.Status400BadRequest, "Invalid data", $"The data violates a database rule ({pg.ConstraintName})."),
        
        _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred", "The server could not process the request.")
    };
}