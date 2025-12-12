namespace ToDoDevSecOpsMinimalAPI.Application.Domain.ToDos;

public class Todo
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    [MaxLength(200)]
    public string? Secret { get; set; } = "This is a secret value";
}
