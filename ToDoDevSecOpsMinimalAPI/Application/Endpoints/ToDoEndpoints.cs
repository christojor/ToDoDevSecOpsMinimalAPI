namespace ToDoDevSecOpsMinimalAPI.Application.Endpoints;

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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public static async Task<IResult> GetAllTodos(IToDoService service)
    {
        var toDos = await service.GetAllAsync();

        return TypedResults.Ok(toDos);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public static async Task<IResult> GetTodo(int id, IToDoService service)
    {
        var item = await service.GetByIdAsync(id);

        return item is not null ? TypedResults.Ok(item) : TypedResults.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Produces(MediaTypeNames.Application.Json)]
    public static async Task<IResult> CreateTodo(TodoCreateDTO todoCreateDTO, IToDoService service, IValidator<TodoCreateDTO> validator)
    {
        var validationResult = await validator.ValidateAsync(todoCreateDTO);

        if (!validationResult.IsValid)
        {
            var errors = ValidationProblemToResult(validationResult);

            return TypedResults.ValidationProblem(errors);
        }

        var createdToDo = await service.CreateAsync(todoCreateDTO);

        return TypedResults.Created($"/todoitems/{createdToDo.Id}", createdToDo);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> UpdateTodo(int id, TodoUpdateDTO todoUpdateDTO, IToDoService service, IValidator<TodoUpdateDTO> validator)
    {
        var validationResult = await validator.ValidateAsync(todoUpdateDTO);

        if (!validationResult.IsValid)
        {
            var errors = ValidationProblemToResult(validationResult);

            return TypedResults.ValidationProblem(errors);
        }

        var success = await service.UpdateAsync(id, todoUpdateDTO);

        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public static async Task<IResult> DeleteTodo(int id, IToDoService service)
    {
        var success = await service.DeleteAsync(id);

        return success ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static Dictionary<string, string[]> ValidationProblemToResult(FluentValidation.Results.ValidationResult validationResult)
    {
        return validationResult.Errors
            .GroupBy(e => e.PropertyName ?? string.Empty)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
    }
}
