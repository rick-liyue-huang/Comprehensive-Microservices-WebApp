using System.Text.Json.Serialization;
using UsersMicroservice.API.Middlewares;
using UsersMicroservice.Core;
using UsersMicroservice.Core.Mappers;
using UsersMicroservice.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddInfrastructure();
builder.Services.AddCore();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddAutoMapper(cfg =>
{
  cfg.LicenseKey = builder.Configuration["AutoMapper:licenseKey"];
}, typeof(ApplicationUserMappingProfile).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
app.Run();
