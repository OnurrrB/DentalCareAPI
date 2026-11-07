using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DentalCareAPI.Errors;

/// <summary>Turns unhandled exceptions into ProblemDetails responses without leaking internals.</summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private const int UniqueIndexViolation = 2601;
    private const int UniqueConstraintViolation = 2627;

    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<GlobalExceptionHandler> logger)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problem = exception switch
        {
            // Two identical requests can both pass the check in the controller; the index decides.
            DbUpdateException { InnerException: SqlException { Number: UniqueIndexViolation or UniqueConstraintViolation } } =>
                new ProblemDetails { Status = StatusCodes.Status409Conflict, Detail = "The record already exists." },
            _ => new ProblemDetails { Status = StatusCodes.Status500InternalServerError, Detail = "An unexpected error occurred." },
        };

        if (problem.Status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception for {Method} {Path}", httpContext.Request.Method, httpContext.Request.Path);

        httpContext.Response.StatusCode = problem.Status!.Value;
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problem,
        });
    }
}
