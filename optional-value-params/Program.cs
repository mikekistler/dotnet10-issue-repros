using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/foo", (int count = 5) =>
{
    return TypedResults.Ok($"Count is {count}");
});

app.MapGet("/bar", ([AsParameters] BarParams barParams) =>
{
    return TypedResults.Ok($"Count is {barParams.Count}");
});

app.MapGet("baz", ([AsParameters] BazParams bazParams) =>
{
    return TypedResults.Ok($"Count is {bazParams.Count}");
});

app.Run();

public class BarParams
{
    [DefaultValue(5)]
    public int Count { get; set; } = 5;
}

public class BazParams(int Count = 5)
{
    [DefaultValue(5)]
    public int Count { get; init; } = Count;
}