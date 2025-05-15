using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

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

var customers = new List<Customer>
{
    new Customer { Id = 1, Name = "John Doe", Email = "johndoe@contoso.com" }
};

app.MapGet("/customers/{id}", Results<Ok<Customer>, NotFound> (int id) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return TypedResults.NotFound();
    }
    return TypedResults.Ok(customer);
});


app.MapPatch("/customers/{id}", Results<Ok<Customer>, NotFound> (int id, JsonPatchDocument<Customer> patchDoc) =>
{
    var customer = customers.FirstOrDefault(c => c.Id == id);
    if (customer == null)
    {
        return TypedResults.NotFound();
    }
    patchDoc.ApplyTo(customer);
    return TypedResults.Ok(customer);
});

app.Run();

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
}
