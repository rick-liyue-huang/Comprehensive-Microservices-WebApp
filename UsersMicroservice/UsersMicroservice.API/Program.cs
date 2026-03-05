using UsersMicroservice.API.Middlewares;
using UsersMicroservice.Infrastructure;
using UsersMicroservice.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure();
builder.Services.AddCore();


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.MapGet("/", () => "Hello World!");

app.Run();