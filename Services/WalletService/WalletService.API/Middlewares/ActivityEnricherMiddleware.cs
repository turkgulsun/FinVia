using System.Diagnostics;

namespace WalletService.API.Middlewares;
public class ActivityEnricherMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var activity = Activity.Current;

        if (activity is not null)
        {
            // TraceId ve SpanId enrich et
            activity.SetTag("trace.id", activity.TraceId);
            activity.SetTag("span.id", activity.SpanId);

            // UserId enrich et (X-User-Id header'ı üzerinden)
            if (context.Request.Headers.TryGetValue("X-User-Id", out var userId))
            {
                activity.SetTag("user.id", userId.ToString());
            }

            // Request bilgilerini enrich et
            activity.SetTag("http.request_method", context.Request.Method);
            activity.SetTag("http.request_path", context.Request.Path);
            activity.SetTag("http.request_host", context.Request.Host);
            activity.SetTag("http.user_agent", context.Request.Headers["User-Agent"].ToString());

            // Opsiyonel: body loglama (güvenlik çok kritikse çıkartabiliriz)
            // context.Request.EnableBuffering();
        }

        await next(context);
    }
}