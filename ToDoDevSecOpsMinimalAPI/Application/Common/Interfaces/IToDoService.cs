namespace ToDoDevSecOpsMinimalAPI.Application.Common.Interfaces;

public interface IToDoService
{
    Task<TodoReadDTO[]> GetAllAsync();
    Task<TodoReadDTO?> GetByIdAsync(int id);
    Task<TodoReadDTO> CreateAsync(TodoCreateDTO dto);
    Task<bool> UpdateAsync(int id, TodoUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}
