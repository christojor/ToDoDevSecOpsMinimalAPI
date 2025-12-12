namespace ToDoDevSecOpsMinimalAPI.Application.Features
{
    public static class ToDoEndpoints
    {
        public static void MapToDoGroup(this WebApplication app) {

            var todoItems = app.MapGroup("/todoitems");

            todoItems.MapGet("/", GetAllTodos);
            todoItems.MapGet("/{id}", GetTodo);
            todoItems.MapPost("/", CreateTodo);
            todoItems.MapPut("/{id}", UpdateTodo);
            todoItems.MapDelete("/{id}", DeleteTodo);
        }

        public static async Task<IResult> GetAllTodos(ToDoDbContext db)
        {
            return TypedResults.Ok(await db.Todos.Select(x => new TodoReadDTO(x)).ToArrayAsync());
        }

        public static async Task<IResult> GetTodo(int id, ToDoDbContext db)
        {
            return await db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(new TodoReadDTO(todo))
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateTodo(TodoCreateDTO todoCreateDTO, ToDoDbContext db)
        {
            var todoItem = new Todo
            {
                IsComplete = todoCreateDTO.IsComplete,
                Name = todoCreateDTO.Name
            };

            db.Todos.Add(todoItem);
            await db.SaveChangesAsync();

            var todoReadDto = new TodoReadDTO(todoItem);

            return TypedResults.Created($"/todoitems/{todoItem.Id}", todoReadDto);
        }

        public static async Task<IResult> UpdateTodo(int id, TodoUpdateDTO todoUpdateDTO, ToDoDbContext db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = todoUpdateDTO.Name;
            todo.IsComplete = todoUpdateDTO.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteTodo(int id, ToDoDbContext db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
