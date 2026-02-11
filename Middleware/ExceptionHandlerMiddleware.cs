using System.Net;
using System.Text.Json;
using PostHubAPI.Exceptions;

namespace PostHubAPI.Middleware;

/// <summary>
/// Middleware that assigns a correlation ID to each request and handles unhandled exceptions
/// by returning a consistent error response and logging.
/// </summary>
public sealed class ExceptionHandlerMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    /// <summary>Initializes a new instance of the <see cref="ExceptionHandlerMiddleware"/> class.</summary>
    /// <param name="next">The next delegate in the pipeline.</param>
    /// <param name="logger">Logger for exception details.</param>
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>Invokes the middleware.</summary>
    /// <param name="context">The HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString("N");
        context.Response.Headers[CorrelationIdHeader] = correlationId;
        context.Items["CorrelationId"] = correlationId;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, correlationId);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex, string correlationId)
    {
        _logger.LogError(ex, "Unhandled exception. CorrelationId: {CorrelationId}", correlationId);

        (HttpStatusCode statusCode, string message) = ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            InvalidOperationException => (HttpStatusCode.InternalServerError, "An internal error occurred."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var body = new ErrorResponseDto
        {
            CorrelationId = correlationId,
            Message = message,
            StatusCode = (int)statusCode
        };

        string json = JsonSerializer.Serialize(body);
        await context.Response.WriteAsync(json);
    }
}

/// <summary>Standard error response body for API exceptions.</summary>
internal sealed class ErrorResponseDto
{
    /// <summary>Correlation ID for tracing the request.</summary>
    public string CorrelationId { get; set; } = string.Empty;

    /// <summary>Human-readable error message.</summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>HTTP status code.</summary>
    public int StatusCode { get; set; }
}
