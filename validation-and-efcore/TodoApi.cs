using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class TodoApi
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/todos");

        group.MapGet("/", async ([AsParameters] TodoServices services) =>
            await services.Db.Todos.ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (int id, [AsParameters] TodoServices services) =>
            await services.Db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound());

        group.MapPost("/", async (Todo todo, [AsParameters] TodoServices services) =>
        {
            services.Db.Todos.Add(todo);
            await services.Db.SaveChangesAsync();

            return TypedResults.Created($"/api/todos/{todo.Id}", todo);
        });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (int id, Todo inputTodo, [AsParameters] TodoServices services) =>
        {
            var todo = await services.Db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Title = inputTodo.Title;
            todo.Description = inputTodo.Description;
            todo.IsComplete = inputTodo.IsComplete;
            todo.DueDate = inputTodo.DueDate;

            if (inputTodo.IsComplete && !todo.CompletedAt.HasValue)
                todo.CompletedAt = DateTime.UtcNow;
            else if (!inputTodo.IsComplete)
                todo.CompletedAt = null;

            await services.Db.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async Task<Results<NoContent, NotFound>> (int id, [AsParameters] TodoServices services) =>
        {
            if (await services.Db.Todos.FindAsync(id) is Todo todo)
            {
                services.Db.Todos.Remove(todo);
                await services.Db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        });
    }
}

public class TodoServices(TodoContext db)
{
    [FromServices]
    public TodoContext Db => db;
}