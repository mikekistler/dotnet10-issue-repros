using deser_error_handling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStatusCodePages();

app.MapPost("/forecast", (WeatherForecast body) => {
    return TypedResults.Ok(body);
});

app.MapControllers();

app.Run();
