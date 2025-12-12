namespace ToDoDevSecOpsMinimalAPI.Application.Features;

using ToDoDevSecOpsMinimalAPI.Application.Common.Interfaces;

public static class ToDoEndpoints
{
    public static void MapToDoGroup(this WebApplication app)
    {
        var todoItems = app.MapGroup("/todoitems");

        todoItems.MapGet("/", GetAllTodos);
        todoItems.MapGet("/{id}", GetTodo);
        todoItems.MapPost("/", CreateTodo);
        todoItems.MapPut("/{id}", UpdateTodo);
        todoItems.MapDelete("/{id}", DeleteTodo);
    }

    public static async Task<IResult> GetAllTodos(IToDoService service)
    {
        var items = await service.GetAllAsync();
        return TypedResults.Ok(items);
    }

    public static async Task<IResult> GetTodo(int id, IToDoService service)
    {
        var item = await service.GetByIdAsync(id);
        return item is not null ? TypedResults.Ok(item) : TypedResults.NotFound();
    }

    public static async Task<IResult> CreateTodo(TodoCreateDTO todoCreateDTO, IToDoService service)
    {
        var created = await service.CreateAsync(todoCreateDTO);
        return TypedResults.Created($"/todoitems/{created.Id}", created);
    }

    public static async Task<IResult> UpdateTodo(int id, TodoUpdateDTO todoUpdateDTO, IToDoService service)
    {
        var ok = await service.UpdateAsync(id, todoUpdateDTO);
        return ok ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    public static async Task<IResult> DeleteTodo(int id, IToDoService service)
    {
        var ok = await service.DeleteAsync(id);
        return ok ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}
