namespace ToDoDevSecOpsMinimalAPI.Tests.UnitTests;

public class ToDoServiceTests
{
    private readonly ToDoDbContext _dbContext;
    private readonly IToDoService _service;

    public ToDoServiceTests()
    {
        _dbContext = new MockDb().CreateDbContext();
        _service = new ToDoService(_dbContext);
    }

    [Fact]
    public async Task GetAllAsync_With_DB_Items_Returns_Items()
    {
        _dbContext.Todos.AddRange(new[] {
            new Todo { Name = "A", IsComplete = false },
            new Todo { Name = "B", IsComplete = true }
        });
        await _dbContext.SaveChangesAsync();

        var items = await _service.GetAllAsync();

        Assert.Equal(2, items.Length);
        Assert.Contains(items, i => i.Name == "A");
        Assert.Contains(items, i => i.Name == "B");
    }

    [Fact]
    public async Task GetAllAsync_With_Empty_DB_Returns_Empty_Array()
    {
        var items = await _service.GetAllAsync();
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task GetAllAsync_Edge_LongNameItemHandled()
    {
        var longName = new string('x', 1000);
        _dbContext.Todos.Add(new Todo { Name = longName, IsComplete = false });
        await _dbContext.SaveChangesAsync();

        var items = await _service.GetAllAsync();
        Assert.Single(items);
        Assert.Equal(1000, items[0].Name.Length);
    }

    [Fact]
    public async Task GetByIdAsync_Positive_ReturnsItem()
    {
        var todo = new Todo { Name = "GetMe", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var found = await _service.GetByIdAsync(todo.Id);

        Assert.NotNull(found);
        Assert.Equal(todo.Id, found!.Id);
        Assert.Equal("GetMe", found.Name);
    }

    [Fact]
    public async Task GetByIdAsync_Negative_ReturnsNullForMissingId()
    {
        var found = await _service.GetByIdAsync(9999);
        Assert.Null(found);
    }

    [Fact]
    public async Task GetByIdAsync_Edge_IdZeroReturnsNull()
    {
        var found = await _service.GetByIdAsync(0);
        Assert.Null(found);
    }

    [Fact]
    public async Task CreateAsync_Positive_CreatesAndReturnsTodo()
    {
        var dto = new TodoCreateDTO { Name = "NewTodo", IsComplete = true };
        var created = await _service.CreateAsync(dto);

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal("NewTodo", created.Name);
        Assert.True(created.IsComplete);

        var fromDb = await _dbContext.Todos.FindAsync(created.Id);
        Assert.NotNull(fromDb);
    }

    [Fact]
    public async Task CreateAsync_Negative_NullDto_Throws()
    {
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.CreateAsync(null!));
    }

    [Fact]
    public async Task CreateAsync_Edge_LongNameIsAccepted()
    {
        var longName = new string('y', 200);
        var dto = new TodoCreateDTO { Name = longName, IsComplete = false };
        var created = await _service.CreateAsync(dto);
        Assert.Equal(200, created.Name.Length);
    }

    [Fact]
    public async Task UpdateAsync_Positive_UpdatesExistingTodo()
    {
        var todo = new Todo { Name = "Before", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var updateDto = new TodoUpdateDTO { Id = todo.Id, Name = "After", IsComplete = true };
        var result = await _service.UpdateAsync(todo.Id, updateDto);

        Assert.True(result);
        var fromDb = await _dbContext.Todos.FindAsync(todo.Id);
        Assert.Equal("After", fromDb!.Name);
        Assert.True(fromDb.IsComplete);
    }

    [Fact]
    public async Task UpdateAsync_Negative_NonExistingReturnsFalse()
    {
        var updateDto = new TodoUpdateDTO { Id = 9999, Name = "X", IsComplete = false };
        var result = await _service.UpdateAsync(9999, updateDto);
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Edge_NullDto_Throws()
    {
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.UpdateAsync(1, null!));
    }

    [Fact]
    public async Task DeleteAsync_Positive_DeletesAndReturnsTrue()
    {
        var todo = new Todo { Name = "ToDelete", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var result = await _service.DeleteAsync(todo.Id);
        Assert.True(result);

        var fromDb = await _dbContext.Todos.FindAsync(todo.Id);
        Assert.Null(fromDb);
    }

    [Fact]
    public async Task DeleteAsync_Negative_NonExistingReturnsFalse()
    {
        var result = await _service.DeleteAsync(9999);
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_Edge_DeleteTwiceSecondReturnsFalse()
    {
        var todo = new Todo { Name = "DoubleDelete", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var first = await _service.DeleteAsync(todo.Id);
        var second = await _service.DeleteAsync(todo.Id);

        Assert.True(first);
        Assert.False(second);
    }
}
