namespace ToDoDevSecOpsMinimalAPI.Tests.UnitTests
{
    public class ToDoEndpointsTests
    {
        private readonly ToDoDbContext _dbContextMock;

        public ToDoEndpointsTests()
        {
            _dbContextMock = new MockDb().CreateDbContext();
        }

        [Fact]
        public async Task GetTodoById_ReturnsOkOfTodoResult()
        {
            // Arrange
            _dbContextMock.Todos.AddRange(
            [
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
            ]);

            // Act
            var result = await ToDoEndpoints.GetTodo(1, _dbContextMock);

            // Assert: Check for the correct returned type
            Assert.IsType<Ok<TodoItemDTO>>(result);
        }

        [Fact]
        public async Task GetAllTodos_ReturnsOkOfTodosResult()
        {
            // Arrange
            _dbContextMock.Todos.AddRange(
            [
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
                new Todo { Id = 2, Name = "Test Todo 2", IsComplete = true }
            ]);

            // Act
            var result = await ToDoEndpoints.GetAllTodos(_dbContextMock);

            // Assert: Check for the correct returned type
            Assert.IsType<Ok<TodoItemDTO[]>>(result);
        }
    }
}
