
using Finvia.Shared.Common;
using FluentValidation;

namespace WalletService.API.Middlewares;

public class ValidationExceptionMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

            var result = Result<string>.Failure(errors.ToArray());

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}