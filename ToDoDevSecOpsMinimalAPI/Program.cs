var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Welcome to the DevSecOps Minimal ToDo API!");
app.MapToDoGroup();

app.Run();