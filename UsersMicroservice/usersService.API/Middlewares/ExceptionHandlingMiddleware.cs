// the original code stored in note.md 

using System.Net;
using System.Text.Json;

namespace usersService.API.Middlewares;

public class ExceptionHandlingMiddleware(
    RequestDelegate next, 
    ILogger<ExceptionHandlingMiddleware> logger,
    IHostEnvironment environment)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 记录完整异常信息到日志
        logger.LogError(exception, 
            "Unhandled exception occurred. TraceId: {TraceId}", 
            context.TraceIdentifier);

        // 根据异常类型确定状态码和消息
        var (statusCode, message) = MapException(exception);

        // 构建响应对象
        var response = new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message,
            TraceId = context.TraceIdentifier
        };

        // 开发环境下添加详细信息
        if (environment.IsDevelopment())
        {
            response.Detail = exception.Message;
            response.ExceptionType = exception.GetType().Name;
            response.StackTrace = exception.StackTrace;
            
            if (exception.InnerException != null)
            {
                response.InnerException = new InnerExceptionInfo
                {
                    Message = exception.InnerException.Message,
                    Type = exception.InnerException.GetType().Name
                };
            }
        }


        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var jsonOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        await context.Response.WriteAsJsonAsync(response, jsonOptions);
    }

    private static (int StatusCode, string Message) MapException(Exception exception)
    {
        return exception switch
        {
            // 400 Bad Request
            ArgumentException or
            ArgumentNullException or
            InvalidOperationException => 
                ((int)HttpStatusCode.BadRequest, exception.Message),

            // 401 Unauthorized
            UnauthorizedAccessException => 
                ((int)HttpStatusCode.Unauthorized, "You are not authorized to access this resource"),

            // 404 Not Found
            KeyNotFoundException or
            FileNotFoundException => 
                ((int)HttpStatusCode.NotFound, "The requested resource was not found"),

            // 409 Conflict
            InvalidDataException => 
                ((int)HttpStatusCode.Conflict, exception.Message),

            // 500 Internal Server Error（默认）
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };
    }
}

/// <summary>
/// Error Response Model
/// </summary>
public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string TraceId { get; set; } = string.Empty;
    
    public string? Detail { get; set; }
    public string? ExceptionType { get; set; }
    public string? StackTrace { get; set; }
    public InnerExceptionInfo? InnerException { get; set; }
}

public class InnerExceptionInfo
{
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

/// <summary>
/// extension method
/// </summary>
public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}