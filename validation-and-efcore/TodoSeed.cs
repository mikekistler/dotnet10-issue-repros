using Microsoft.EntityFrameworkCore;

public static class TodoSeed
{
    public static async Task SeedDatabaseAsync(WebApplication app)
    {
        // Create and seed the database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                var context = services.GetRequiredService<TodoContext>();
                context.Database.EnsureCreated();

                await SeedAsync(context, app.Environment);
                logger.LogInformation("Catalog seeding complete.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while initializing the database.");
            }
        }
    }
    public static async Task SeedAsync(TodoContext context, IWebHostEnvironment env)
    {
        // Only seed if the database is empty
        if (await context.Todos.AnyAsync())
        {
            return;
        }

        var todos = new List<Todo>
        {
            new Todo
            {
                Title = "Learn .NET 10",
                Description = "Study the new features in .NET 10",
                IsComplete = false,
                DueDate = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            },
            new Todo
            {
                Title = "Build a minimal API",
                Description = "Create a REST API using .NET minimal API approach",
                IsComplete = true,
                DueDate = DateTime.UtcNow.AddDays(-3),
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                CompletedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Todo
            {
                Title = "Implement Entity Framework Core",
                Description = "Set up EF Core with SQLite in the project",
                IsComplete = false,
                DueDate = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Todo
            {
                Title = "Write unit tests",
                Description = "Create comprehensive test suite for the API",
                IsComplete = false,
                DueDate = DateTime.UtcNow.AddDays(14),
                CreatedAt = DateTime.UtcNow
            },
            new Todo
            {
                Title = "Deploy to production",
                Description = "Deploy the application to the production environment",
                IsComplete = false,
                DueDate = DateTime.UtcNow.AddDays(30),
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Todos.AddRangeAsync(todos);
        await context.SaveChangesAsync();
    }
}
