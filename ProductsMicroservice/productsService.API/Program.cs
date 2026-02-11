using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using productsService.API.APIEndpoints;
using productsService.API.Middlewares;
using productsService.BusinessLogicLayer;
using productsService.DataAccessLayer;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper
builder.Services.AddAutoMapper(cfg =>
{
    cfg.LicenseKey = builder.Configuration["LuckyPenny:AutoMapperLicenseKey"];
}, typeof(Program).Assembly);

builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddBusinessLogicLayer();

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();


// because here has the enum type, so it will work on post method.
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        b => b.WithOrigins("http://localhost:3000")
            .AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseExceptionHandlingMiddleware();
app.UseRouting();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapProductAPIEndpoints();

app.Run();