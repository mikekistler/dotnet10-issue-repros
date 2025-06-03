#undef CONTROLLERS

using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

using deser_error_handling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#if CONTROLLERS
builder.Services.AddControllers();
#endif

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonContext.Default);
});

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<JsonExceptionHandler>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.MapPost("/forecast", (WeatherForecast body) => {
    return TypedResults.Ok(body);
});

#if CONTROLLERS
app.MapControllers();
#endif

app.Run();

[JsonSerializable(typeof(WeatherForecast))]
[JsonSerializable(typeof(ValidationProblemDetails))]
internal partial class AppJsonContext : JsonSerializerContext
{
}