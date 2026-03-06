using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
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

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(b =>
    {
        // b.AllowAnyOrigin();
        b.WithOrigins(["http://localhost:3000", "http://localhost:4200", "http://localhost:5000"]);
        b.AllowAnyMethod();
        b.AllowAnyHeader();
    });
});


var app = builder.Build();

app.UseExceptionHandlingMiddleware();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// app.MapGet("/", () => "Hello World!");

app.Run();