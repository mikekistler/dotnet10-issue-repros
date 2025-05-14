using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("/foo", GetFoo).WithName("GetFoo");

app.MapGet("/bar", GetBar).WithName("GetBar");

app.MapPost("/baz", PostBaz).WithName("PostBaz");

app.Run();

partial class Program
{
    /// <summary>
    /// Get a foo
    /// </summary>
    /// <remarks>
    /// Get a foo with a <c>foo</c> parameter.
    /// </remarks>
    /// <param name="foo">This is a foo</param>
    public static Ok<string> GetFoo(string? foo)
    {
        return TypedResults.Ok(foo);
    }

    /// <summary>
    /// Get a bar
    /// </summary>
    /// <remarks>
    /// Get a bar with a <c>bar</c> parameter.
    /// </remarks>
    /// <param name="barParams">Parameters for Bar</param>
    public static Ok<string> GetBar([AsParameters] BarBaz barParams)
    {
        return TypedResults.Ok(barParams.Bar);
    }

    /// <summary>
    /// Post a baz
    /// </summary>
    /// <remarks>
    /// Post a baz with a <c>baz</c> parameter.
    /// </remarks>
    /// <param name="bazParams">Parameters for Baz</param>
    public static Ok<string> PostBaz([FromBody] BarBaz bazParams)
    {
        return TypedResults.Ok(bazParams.Baz);
    }
}

public class BarBaz
{
    /// <summary>This is a bar</summary>
    public string? Bar { get; set; }
    /// <summary>This is a baz</summary>
    public string? Baz { get; set; }
}