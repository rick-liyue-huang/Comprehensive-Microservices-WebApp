using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
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

builder.Services.AddFluentValidationAutoValidation();

// add API explorer services
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(x =>
    {
        x
            .WithOrigins("http://localhost:3000", "http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseSwagger(); // Adds Endpoint that can serve the swagger.json file
app.UseSwaggerUI(); // Adds swagger UI PAGE at /swagger
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();