var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173", "http://localhost:5175")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevSecOps Minimal ToDo API",
        Version = "v1",
        Description = "A minimal ToDo API demonstrating DevSecOps practices using ASP.NET Core.\n\n" +
                      "Repository: [ToDoDevSecOpsMinimalAPI](https://github.com/christojor/ToDoDevSecOpsMinimalAPI)\n\n" +
                      "Frontend: [ToDoDevSecOpsGUI](https://github.com/christojor/ToDoDevSecOpsGUI)",
        Contact = new OpenApiContact
        {
            Name = "Support",
            Email = "christoffer.joergensen@gmail.com"
        }
    });
});

var app = builder.Build();

app.MapToDoGroup();

app.UsePathBase("/api");
app.UseCors();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();