using System.Text.Json.Serialization;
using usersService.API.Middlewares;
using usersService.Core;
using usersService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureService();
builder.Services.AddCoreService();

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();