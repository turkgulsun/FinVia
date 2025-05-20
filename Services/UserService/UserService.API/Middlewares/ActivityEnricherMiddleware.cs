using System.Diagnostics;

namespace UserService.API.Middlewares;
public class ActivityEnricherMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var activity = Activity.Current;

        if (activity is not null)
        {
            activity.SetTag("http.request_method", context.Request.Method);
            activity.SetTag("http.request_path", context.Request.Path);
            activity.SetTag("user_agent", context.Request.Headers.UserAgent.ToString());
            activity.SetTag("trace_id", activity.TraceId.ToString());
            activity.SetTag("span_id", activity.SpanId.ToString());
        }

        await next(context);
    }
}