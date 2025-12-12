namespace ToDoDevSecOpsMinimalAPI.Tests.UnitTests
{
    using System.Threading.Tasks;
    using Xunit;
    using Microsoft.AspNetCore.Http.HttpResults;
    using ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Persistence;
    using ToDoDevSecOpsMinimalAPI.Application.Domain.ToDos;
    using ToDoDevSecOpsMinimalAPI.Application.Common.Models;
    using ToDoDevSecOpsMinimalAPI.Application.Features;
    using ToDoDevSecOpsMinimalAPI.Application.Infrastructure.Services;

    public class ToDoEndpointsTests
    {
        private readonly ToDoDbContext _dbContext;
        private readonly IToDoService _service;

        public ToDoEndpointsTests()
        {
            _dbContext = new MockDb().CreateDbContext();
            _service = new ToDoService(_dbContext);
        }

        [Fact]
        public async Task GetTodoById_Returns_OK_Result()
        {
            // Given a new Todo item in the mock database
            _dbContext.Todos.AddRange(new[]
            {
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false }
            });
            await _dbContext.SaveChangesAsync();

            // When retrieving the Todo item by ID
            var result = await ToDoEndpoints.GetTodo(1, _service);

            // Then the result should be of type Ok with TodoReadDTO
            var ok = Assert.IsType<Ok<TodoReadDTO>>(result);
            Assert.Equal(1, ok.Value.Id);
            Assert.Equal("Test Todo 1", ok.Value.Name);
        }

        [Fact]
        public async Task GetAllTodos_Returns_OK_Result()
        {
            // Given multiple Todo items in the mock database
            _dbContext.Todos.AddRange(new[]
            {
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
                new Todo { Id = 2, Name = "Test Todo 2", IsComplete = true }
            });
            await _dbContext.SaveChangesAsync();

            // When retrieving all Todo items
            var result = await ToDoEndpoints.GetAllTodos(_service);

            // Then the result should be of type Ok with TodoReadDTO[]
            var ok = Assert.IsType<Ok<TodoReadDTO[]>>(result);
            Assert.Equal(2, ok.Value.Length);
        }
    }
}
