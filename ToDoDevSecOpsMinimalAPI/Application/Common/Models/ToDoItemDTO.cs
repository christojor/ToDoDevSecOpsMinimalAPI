namespace ToDoDevSecOpsMinimalAPI.Application.Common.Models;

public class TodoCreateDTO
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    public TodoCreateDTO() { }
}

public class TodoReadDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    public TodoReadDTO() { }
    public TodoReadDTO(Todo todo) => (Id, Name, IsComplete) = (todo.Id, todo.Name, todo.IsComplete);
}

public class TodoUpdateDTO
{
    [Required]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    public TodoUpdateDTO() { }
}
