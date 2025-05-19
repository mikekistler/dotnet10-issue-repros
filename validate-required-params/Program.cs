using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddValidation();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/required-param", (
    [Required] string? name
) =>
{
    return TypedResults.Ok($"Hello {name}");
});

app.MapGet("/minlength-param", (
    [MaxLength(5)] string? name
) =>
{
    return TypedResults.Ok($"Hello {name}");
});

app.MapGet("/path-param/{name}", (
    [MaxLength(5)] string? name
) =>
{
    return TypedResults.Ok($"Hello {name}");
});

app.MapGet("/int-path-param/{id}", (
    [Range(42,42)] int id
) =>
{
    return TypedResults.Ok($"Hello {id}");
});

app.Run();
