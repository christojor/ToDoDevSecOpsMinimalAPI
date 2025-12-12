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
        public async Task GetTodoById_Returns_OK_Result()
        {
            // Given a new Todo item in the mock database
            _dbContextMock.Todos.AddRange(
            [
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
            ]);

            // When retrieving the Todo item by ID
            var result = await ToDoEndpoints.GetTodo(1, _dbContextMock);

            // Then the result should be of type Ok<TodoItemDTO>
            Assert.IsType<Ok<TodoItemDTO>>(result);
        }

        [Fact]
        public async Task GetAllTodos_Returns_OK_Result()
        {
            // Given multiple Todo items in the mock database
            _dbContextMock.Todos.AddRange(
            [
                new Todo { Id = 1, Name = "Test Todo 1", IsComplete = false },
                new Todo { Id = 2, Name = "Test Todo 2", IsComplete = true }
            ]);

            // When retrieving all Todo items
            var result = await ToDoEndpoints.GetAllTodos(_dbContextMock);

            // Then the result should be of type Ok<TodoItemDTO[]>
            Assert.IsType<Ok<TodoItemDTO[]>>(result);
        }
    }
}
