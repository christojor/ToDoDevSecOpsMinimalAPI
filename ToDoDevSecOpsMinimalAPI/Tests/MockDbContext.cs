namespace ToDoDevSecOpsMinimalAPI.Tests;

public class MockDb : IDbContextFactory<ToDoDbContext>
{
    public ToDoDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ToDoDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new ToDoDbContext(options);
    }
}
