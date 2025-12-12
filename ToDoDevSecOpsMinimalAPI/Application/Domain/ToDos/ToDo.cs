namespace ToDoDevSecOpsMinimalAPI.Application.Domain.ToDos;

public class Todo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
    public string? Secret { get; set; } = "This is a secret value";
}
