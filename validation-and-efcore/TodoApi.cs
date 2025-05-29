using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

public static class TodoApi
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/todos");

        group.MapGet("/", async (TodoContext db) =>
            await db.Todos.ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (int id, TodoContext db) =>
            await db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound());

        group.MapPost("/", async (Todo todo, TodoContext db) =>
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/todos/{todo.Id}", todo);
        });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (int id, Todo inputTodo, TodoContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Title = inputTodo.Title;
            todo.Description = inputTodo.Description;
            todo.IsComplete = inputTodo.IsComplete;
            todo.DueDate = inputTodo.DueDate;

            if (inputTodo.IsComplete && !todo.CompletedAt.HasValue)
                todo.CompletedAt = DateTime.UtcNow;
            else if (!inputTodo.IsComplete)
                todo.CompletedAt = null;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, TodoContext db) =>
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        });
    }
}