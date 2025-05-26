using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Finvia.Shared.Middlewares;

public class CorrelationIdMiddleware
{
    private const string HeaderKey = "X-Correlation-ID";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderKey, out var cid)
            ? cid.ToString()
            : Guid.NewGuid().ToString();

        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers[HeaderKey] = correlationId;

        using (LogContext.PushProperty("CorrelationId", correlationId)) // Serilog integration
        {
            await _next(context);
        }
    }
}