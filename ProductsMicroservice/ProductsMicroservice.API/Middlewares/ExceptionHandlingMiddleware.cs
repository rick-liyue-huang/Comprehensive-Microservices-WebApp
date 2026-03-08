namespace ProductsMicroservice.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            if (e.InnerException != null)
            {
                logger.LogError("{ExceptionType} {ExceptionMessage}", e.InnerException.GetType().Name, e.InnerException.Message);
            }
            else
            {
                logger.LogError("{ExceptionType} {ExceptionMessage}", e.GetType().Name, e.Message);
            }
            
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Message = e.Message, Type = e.GetType().Name });
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