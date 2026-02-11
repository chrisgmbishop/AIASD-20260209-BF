namespace PostHubAPI.Middleware;

/// <summary>
/// Middleware that ensures a correlation ID is present on the request for tracing.
/// ExceptionHandlerMiddleware already sets one; this allows reading it from HttpContext.Items elsewhere.
/// </summary>
public static class CorrelationIdMiddleware
{
    /// <summary>Header name used for correlation ID.</summary>
    public const string CorrelationIdHeader = "X-Correlation-ID";

    /// <summary>Gets the correlation ID for the current request from <see cref="HttpContext.Items"/>.</summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>The correlation ID, or null if not set.</returns>
    public static string? GetCorrelationId(HttpContext context)
    {
        return context?.Items["CorrelationId"] as string;
    }
}
