namespace ToDoDevSecOpsMinimalAPI.Tests.UnitTests;

public class ToDoEndpointTests
{
    private readonly ToDoDbContext _dbContext;
    private readonly IToDoService _service;

    public ToDoEndpointTests()
    {
        _dbContext = new MockDb().CreateDbContext();
        _service = new ToDoService(_dbContext);
    }

    [Fact]
    public async Task GetTodoById_Returns_OK_Result()
    {
        // Given a new Todo item in the mock database
        _dbContext.Todos.AddRange(new[]
        {
            new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false }
        });
        await _dbContext.SaveChangesAsync();

        // When retrieving the Todo item by ID
        var result = await ToDoEndpoints.GetTodo(1, _service);

        // Then the result should be of type Ok with TodoReadDTO
        var ok = Assert.IsType<Ok<TodoReadDTO>>(result);
        Assert.Equal(1, ok.Value!.Id);
        Assert.Equal("Test Todo 1", ok.Value.Name);
    }

    [Fact]
    public async Task GetAllTodos_Returns_OK_Result()
    {
        // Given multiple Todo items in the mock database
        _dbContext.Todos.AddRange(new[]
        {
            new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
            new Todo { Id = 2, Name = "Test Todo 2", IsComplete = true }
        });
        await _dbContext.SaveChangesAsync();

        // When retrieving all Todo items
        var result = await ToDoEndpoints.GetAllTodos(_service);

        // Then the result should be of type Ok with TodoReadDTO[]
        var ok = Assert.IsType<Ok<TodoReadDTO[]>>(result);
        Assert.Equal(2, ok.Value!.Length);
    }

    [Fact]
    public async Task CreateTodo_Returns_Created_Result()
    {
        // Given a valid create DTO
        var createDto = new TodoCreateDTO { Name = "walk dog", IsComplete = true };

        // When creating the Todo
        var result = await ToDoEndpoints.CreateTodo(createDto, _service);

        // Then the result should be Created with the created DTO
        var created = Assert.IsType<Created<TodoReadDTO>>(result);
        Assert.Equal("walk dog", created.Value!.Name);
        Assert.True(created.Value.IsComplete);
        Assert.True(created.Value.Id > 0);
    }

    [Fact]
    public async Task UpdateTodo_Positive_Returns_NoContent()
    {
        // Given an existing Todo in the database
        var todo = new Todo { Name = "Before", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var updateDto = new TodoUpdateDTO { Id = todo.Id, Name = "After", IsComplete = true };

        // When updating the Todo
        var result = await ToDoEndpoints.UpdateTodo(todo.Id, updateDto, _service);

        // Then the result should be NoContent and the DB should be updated
        Assert.IsType<NoContent>(result);
        var fromDb = await _dbContext.Todos.FindAsync(todo.Id);
        Assert.Equal("After", fromDb!.Name);
        Assert.True(fromDb.IsComplete);
    }

    [Fact]
    public async Task UpdateTodo_Negative_Returns_NotFound()
    {
        // Given no Todo with the id exists
        var updateDto = new TodoUpdateDTO { Id = 9999, Name = "X", IsComplete = false };

        // When attempting to update
        var result = await ToDoEndpoints.UpdateTodo(9999, updateDto, _service);

        // Then NotFound is returned
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task DeleteTodo_Positive_Returns_NoContent()
    {
        // Given an existing Todo in the database
        var todo = new Todo { Name = "ToDelete", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        // When deleting the Todo
        var result = await ToDoEndpoints.DeleteTodo(todo.Id, _service);

        // Then the result should be NoContent and the item removed
        Assert.IsType<NoContent>(result);
        var fromDb = await _dbContext.Todos.FindAsync(todo.Id);
        Assert.Null(fromDb);
    }

    [Fact]
    public async Task DeleteTodo_Negative_Returns_NotFound()
    {
        // Given no Todo exists for the provided id

        // When deleting
        var result = await ToDoEndpoints.DeleteTodo(9999, _service);

        // Then NotFound is returned
        Assert.IsType<NotFound>(result);
    }
}
