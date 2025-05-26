using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Finvia.Shared.Middlewares;

public class ActivityEnricherMiddleware(RequestDelegate next, ILogger<ActivityEnricherMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var activity = Activity.Current;

        if (activity != null)
        {
            if (context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
            {
                activity.SetTag("correlation_id", correlationId.ToString());
            }

            activity.SetTag("http.method", context.Request.Method);
            activity.SetTag("http.url", context.Request.Path);
        }

        await next(context);
    }
}