namespace ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Services;

public class ToDoService : IToDoService
{
    private readonly ToDoDbContext _db;

    public ToDoService(ToDoDbContext db)
    {
        _db = db;
    }

    public async Task<TodoReadDTO[]> GetAllAsync()
    {
        return await _db.Todos.Select(t => new TodoReadDTO(t)).ToArrayAsync();
    }

    public async Task<TodoReadDTO?> GetByIdAsync(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        return todo is null ? null : new TodoReadDTO(todo);
    }

    public async Task<TodoReadDTO> CreateAsync(TodoCreateDTO dto)
    {
        if (dto is null) throw new NullReferenceException();
        if (dto.Name?.Length > 100) throw new ArgumentException("Name exceeds maximum length of 100 characters", nameof(dto.Name));

        var todo = new Todo { Name = dto.Name, IsComplete = dto.IsComplete };
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
        return new TodoReadDTO(todo);
    }

    public async Task<bool> UpdateAsync(int id, TodoUpdateDTO dto)
    {
        if (dto is null) throw new NullReferenceException();

        var todo = await _db.Todos.FindAsync(id);
        if (todo is null) return false;
        todo.Name = dto.Name;
        todo.IsComplete = dto.IsComplete;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null) return false;
        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
        return true;
    }
}
