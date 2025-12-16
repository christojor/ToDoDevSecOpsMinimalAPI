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
        // Ensure DB is empty (clear any cross-test data in the in-memory database)
        _dbContext.Todos.RemoveRange(_dbContext.Todos);
        await _dbContext.SaveChangesAsync();

        var items = await _service.GetAllAsync();
        Assert.NotNull(items);
        Assert.Empty(items);
    }

    [Fact]
    public async Task Cannot_Create_ToDo_With_Name_Exceeding_MaxLength()
    {
        var longName = new string('z', 101);
        var dto = new TodoCreateDTO { Name = longName, IsComplete = false };

        await Assert.ThrowsAsync<ArgumentException>(async () => await _service.CreateAsync(dto));
    }

    [Fact]
    public async Task GetByIdAsync_With_DB_Item_Returns_Item()
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
    public async Task GetByIdAsync_Without_DB_Item_Returns_Null()
    {
        var found = await _service.GetByIdAsync(9999);
        Assert.Null(found);
    }

    [Fact]
    public async Task GetByIdAsync_With_Invalid_Id_Returns_Null()
    {
        var found = await _service.GetByIdAsync(0);
        Assert.Null(found);
    }

    [Fact]
    public async Task CreateAsync_Creates_And_Returns_Todo()
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
    public async Task CreateAsync_With_No_Input_Throws_Exception()
    {
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.CreateAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_Updates_Existing_Todo()
    {
        var todo = new Todo { Name = "Before", IsComplete = false };
        _dbContext.Todos.Add(todo);
        await _dbContext.SaveChangesAsync();

        var updateDto = new TodoUpdateDTO { Name = "After", IsComplete = true };
        var result = await _service.UpdateAsync(todo.Id, updateDto);

        Assert.True(result);
        var fromDb = await _dbContext.Todos.FindAsync(todo.Id);
        Assert.Equal("After", fromDb!.Name);
        Assert.True(fromDb.IsComplete);
    }

    [Fact]
    public async Task UpdateAsync_Cannot_Update_Non_Existing_Todo()
    {
        var updateDto = new TodoUpdateDTO { Name = "X", IsComplete = false };
        var result = await _service.UpdateAsync(9999, updateDto);
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Cannot_Update_With_Null_Input_Throws_Exception()
    {
        await Assert.ThrowsAsync<NullReferenceException>(async () => await _service.UpdateAsync(1, null!));
    }

    [Fact]
    public async Task DeleteAsync_Deletes_ToDo_And_Returns_True()
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
    public async Task DeleteAsync_Cannot_Delete_Non_Existing_Todo()
    {
        var result = await _service.DeleteAsync(9999);
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_Cannot_Delete_Already_Deleted_Todo()
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
