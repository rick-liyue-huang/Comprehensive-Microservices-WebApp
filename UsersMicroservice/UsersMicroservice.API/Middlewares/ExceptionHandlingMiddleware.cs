namespace UsersMicroservice.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{ex.GetType().ToString()}:  {ex.Message}");

            if (ex.InnerException != null)
            {
                logger.LogError(ex.InnerException, $"{ex.InnerException.GetType().ToString()}:  {ex.InnerException.Message}");
            }
            
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Message = ex.Message, Type = ex.GetType().ToString()});
        }
    }
}


// Extension method used to add the middleware to the HTTP request pipeline.
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}