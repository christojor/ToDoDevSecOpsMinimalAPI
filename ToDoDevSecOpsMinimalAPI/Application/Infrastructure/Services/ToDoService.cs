namespace ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Services;

public class ToDoService : IToDoService
{
    private readonly ToDoDbContext _context;

    public ToDoService(ToDoDbContext context)
    {
        _context = context;
    }

    public async Task<TodoReadDTO[]> GetAllAsync()
    {
        return await _context.Todos.Select(t => new TodoReadDTO(t)).ToArrayAsync();
    }

    public async Task<TodoReadDTO?> GetByIdAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        return todo is null ? null : new TodoReadDTO(todo);
    }

    public async Task<TodoReadDTO> CreateAsync(TodoCreateDTO dto)
    {
        if (dto is null) throw new NullReferenceException();
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required", nameof(dto.Name));
        if (dto.Name.Length > 100) throw new ArgumentException("Name exceeds maximum length of 100 characters", nameof(dto.Name));

        var name = dto.Name; // local non-null copy
        var todo = new Todo { Name = name, IsComplete = dto.IsComplete };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();
        return new TodoReadDTO(todo);
    }

    public async Task<bool> UpdateAsync(int id, TodoUpdateDTO dto)
    {
        if (dto is null) throw new NullReferenceException();
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required", nameof(dto.Name));
        if (dto.Name.Length > 100) throw new ArgumentException("Name exceeds maximum length of 100 characters", nameof(dto.Name));

        var todo = await _context.Todos.FindAsync(id);
        if (todo is null) return false;
        todo.Name = dto.Name;
        todo.IsComplete = dto.IsComplete;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo is null) return false;
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return true;
    }
}
