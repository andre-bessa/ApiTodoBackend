namespace ApiTodoBackend.Models;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetTodos();

    Task<Todo?> GetTodoById(int id);

    Task DeleteTodos();

    Task DeleteTodo(Todo todo);

    Task CreateTodo(Todo todo);

    Task UpdateTodo(Todo todo);

    Task SaveChanges();
}
