using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace UsersMicroservice.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // 1. Log the full exception object to preserve the complete Stack Trace
            logger.LogError(ex, "An unhandled exception occurred while processing the request.");

            // 2. Prepare base response parameters
            context.Response.ContentType = "application/json";
            int statusCode = 500;
            string friendlyMessage = "An unexpected internal server error occurred. Please try again later.";

            // 3. Dynamically set HTTP status codes based on the exception type
            // Note: You will need to define CustomValidationException and NotFoundException in your Core layer
            /* if (ex is CustomValidationException) 
            {
                statusCode = 400; // Bad Request
                friendlyMessage = ex.Message; // Safe to expose business validation errors to the React frontend
            }
            else if (ex is NotFoundException)
            {
                statusCode = 404; // Not Found
                friendlyMessage = ex.Message; 
            }
            */

            context.Response.StatusCode = statusCode;

            // 4. Return a standardized JSON response, hiding sensitive internal details
            await context.Response.WriteAsJsonAsync(new
            {
                Status = statusCode,
                Error = ex.GetType().Name,
                Message = friendlyMessage 
                // Detail = ex.Message // Uncomment only in the Development environment for debugging!
            });
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}