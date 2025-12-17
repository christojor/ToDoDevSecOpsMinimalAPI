namespace ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Services;

public class ToDoService(ToDoDbContext context) : IToDoService
{
    private readonly ToDoDbContext _context = context;

    public async Task<TodoReadDTO[]> GetAllAsync()
    {
        return await _context.Todos.Select(_ => new TodoReadDTO(_)).ToArrayAsync();
    }

    public async Task<TodoReadDTO?> GetByIdAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);

        return todo is null ? null : new TodoReadDTO(todo);
    }

    public async Task<TodoReadDTO> CreateAsync(TodoCreateDTO dto)
    {
        var todo = new Todo { Name = dto.Name, IsComplete = dto.IsComplete };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return new TodoReadDTO(todo);
    }

    public async Task<bool> UpdateAsync(int id, TodoUpdateDTO dto)
    {
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
