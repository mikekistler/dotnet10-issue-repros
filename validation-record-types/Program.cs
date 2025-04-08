#define USE_RECORD

using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/todos", (Todo todo) =>
{
    return Results.Created($"/todos/{todo.Id}", todo);
});

app.Run();

#if USE_RECORD
record Todo(
    int Id,
    [property: Required]
    [property: StringLength(100, MinimumLength = 3)]
    string? Title,
    bool IsComplete
) {}
#else
class Todo
{
    public int Id { get; init; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string? Title { get; init; }

    public bool IsComplete { get; init; }
}
#endif
