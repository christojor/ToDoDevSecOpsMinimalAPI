namespace ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoDevSecOpsMinimalAPI.Application.Domain.ToDos;

public class ToDoDbContext : DbContext
{
    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
