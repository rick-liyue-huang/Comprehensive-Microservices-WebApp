using usersService.API.Middlewares;
using usersService.Core;
using usersService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureService();
builder.Services.AddCoreService();

builder.Services.AddControllers();


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();