using System.Text.Json.Serialization;
using UsersMicroservice.API.Middlewares;
using UsersMicroservice.Infrastructure;
using UsersMicroservice.Core;
using UsersMicroservice.Core.Mappers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore(builder.Configuration);

builder.Services.AddAutoMapper(ctf => {}, typeof(ApplicationUserMappingProfile));


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// app.MapGet("/", () => "Hello World!");

app.Run();