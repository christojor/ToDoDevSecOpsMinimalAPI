namespace ToDoDevSecOpsMinimalAPI.Tests.UnitTests
{
    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.AspNetCore.Http.HttpResults;
    using ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Persistence;
    using ToDoDevSecOpsMinimalAPI.Application.Domain.ToDos;
    using ToDoDevSecOpsMinimalAPI.Application.Common.Models;
    using ToDoDevSecOpsMinimalAPI.Application.Features;

    public class ToDoEndpointsTests
    {
        private readonly ToDoDbContext _dbContextMock;

        public ToDoEndpointsTests()
        {
            _dbContextMock = new MockDb().CreateDbContext();
        }

        [Fact]
        public async Task GetTodoById_Returns_OK_Result()
        {
            // Given a new Todo item in the mock database
            _dbContextMock.Todos.AddRange(new[]
            {
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false }
            });
            await _dbContextMock.SaveChangesAsync();

            // When retrieving the Todo item by ID
            var result = await ToDoEndpoints.GetTodo(1, _dbContextMock);

            // Then the result should be of type Ok with TodoReadDTO
            var ok = Assert.IsType<Ok<TodoReadDTO>>(result);
            Assert.Equal(1, ok.Value.Id);
            Assert.Equal("Test Todo 1", ok.Value.Name);
        }

        [Fact]
        public async Task GetAllTodos_Returns_OK_Result()
        {
            // Given multiple Todo items in the mock database
            _dbContextMock.Todos.AddRange(new[]
            {
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
                new Todo { Id = 2, Name = "Test Todo 2", IsComplete = true }
            });
            await _dbContextMock.SaveChangesAsync();

            // When retrieving all Todo items
            var result = await ToDoEndpoints.GetAllTodos(_dbContextMock);

            // Then the result should be of type Ok with TodoReadDTO[]
            var ok = Assert.IsType<Ok<TodoReadDTO[]>>(result);
            Assert.Equal(2, ok.Value.Length);
        }
    }
}
