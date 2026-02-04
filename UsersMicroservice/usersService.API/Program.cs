using System.Text.Json.Serialization;
using usersService.API.Middlewares;
using usersService.Core;
using usersService.Core.Mappers;
using usersService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureService();
builder.Services.AddCoreService();

builder.Services.AddControllers().AddJsonOptions(
    options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));


// if we use automapper, we have to register it here
builder.Services.AddAutoMapper(
    cfg => {},
    typeof(ApplicationUserMappingProfile).Assembly, 
    typeof(ApplicationUserMappingProfile).Assembly); // if come from different assemblies, it's necessary to add them here,and otherwise it can be omitted.


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();