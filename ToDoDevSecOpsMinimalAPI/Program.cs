var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ToDoDbContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddScoped<IToDoService, ToDoService>();
builder.Services.AddScoped<IValidator<TodoCreateDTO>, CreateToDoValidator>();
builder.Services.AddScoped<IValidator<TodoUpdateDTO>, UpdateToDoValidator>();
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
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapToDoGroup();

app.UsePathBase("/api");
app.UseCors();
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();