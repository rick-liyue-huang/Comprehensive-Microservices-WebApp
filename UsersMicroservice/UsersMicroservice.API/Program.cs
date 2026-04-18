using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
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

builder.Services.AddAutoMapper(cfg => { cfg.LicenseKey = builder.Configuration["AutoMapper:licenseKey"]; },
  typeof(ApplicationUserMappingProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll",
      policy =>
      {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
      });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllers();

app.Run();
