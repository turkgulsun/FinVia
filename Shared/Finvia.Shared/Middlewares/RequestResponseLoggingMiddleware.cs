using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Finvia.Shared.Middlewares;

public class RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
{
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Request log
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true).ReadToEndAsync();
        context.Request.Body.Position = 0;

        logger.LogInformation("📥 HTTP REQUEST: {Method} {Path} | Body: {Body}", 
            context.Request.Method, 
            context.Request.Path,
            string.IsNullOrWhiteSpace(requestBody) ? "(empty)" : requestBody);

        // Response log
        var originalBodyStream = context.Response.Body;
        await using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await next(context);

        newBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();
        newBodyStream.Seek(0, SeekOrigin.Begin);
        await newBodyStream.CopyToAsync(originalBodyStream);

        logger.LogInformation("📤 HTTP RESPONSE: {StatusCode} | Body: {Body}",
            context.Response.StatusCode,
            string.IsNullOrWhiteSpace(responseBody) ? "(empty)" : responseBody);
    }
}