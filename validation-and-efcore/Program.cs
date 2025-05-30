using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add validation services to the DI container.
builder.Services.AddValidation();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add SQLite database
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite("Data Source=todos.db"));

// Add JsonSerializerContext for AOT compilation
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(TodoJsonContext.Default);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Apply migrations at startup when in development
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<TodoContext>();
        db.Database.EnsureCreated();
    }

    // Seed the database with initial data
    await TodoSeed.SeedDatabaseAsync(app);
}

app.UseHttpsRedirection();

// Map Todo API endpoints
app.MapTodoEndpoints();

app.Run();
