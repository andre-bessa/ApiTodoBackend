using ApiTodoBackend.Models;
using System.Collections.Concurrent;

namespace ApiTodoBackend.Data;

public class InMemoryTodoRepository : ITodoRepository
{
    private static readonly object _locker = new();
    private static readonly ConcurrentDictionary<int, Todo> _db = new();
    private static int _nextId = 1;

    public Task CreateTodo(Todo todo)
    {
        lock (_locker)
        {
            todo.TodoId = _nextId++;
            _db.TryAdd(todo.TodoId, todo);
            return Task.CompletedTask;
        }
    }

    public Task DeleteTodo(Todo todo)
    {
        _db.TryRemove(todo.TodoId, out _);
        return Task.CompletedTask;
    }

    public Task DeleteTodos()
    {
        _db.Clear();
        return Task.CompletedTask;
    }

    public Task<Todo?> GetTodoById(int id)
    {
        if (_db.TryGetValue(id, out Todo? todo))
        {
            return Task.FromResult<Todo?>(todo);
        }
        else
        {
            return Task.FromResult<Todo?>(null);
        }
    }

    public Task<IEnumerable<Todo>> GetTodos()
    {
        return Task.FromResult<IEnumerable<Todo>>(_db.Values);
    }

    public Task SaveChanges()
    {
        return Task.CompletedTask;
    }

    public Task UpdateTodo(Todo todo)
    {
        _db[todo.TodoId] = todo;
        return Task.CompletedTask;
    }
}
