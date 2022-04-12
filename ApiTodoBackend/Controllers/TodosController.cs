using ApiTodoBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiTodoBackend.Controllers;

[Route("[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Consumes("application/json")]
[Produces("application/json")]
public class TodosController : ControllerBase
{
    private readonly ITodoRepository _repo;

    public TodosController(ITodoRepository repo)
    {
        _repo = repo;
    }

    [HttpPost(Name = nameof(PostTodo))]
    public async Task<ActionResult<TodoDTO>> PostTodo(TodoDTO reqDto)
    {
        Todo todo = MapDTO2Todo(reqDto);
        await _repo.CreateTodo(todo);
        await _repo.SaveChanges();
        TodoDTO resDto = MapTodo2DTO(todo);
        return Created(resDto.Url!, resDto);
    }

    [HttpGet(Name = nameof(GetAllTodos))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IEnumerable<TodoDTO>> GetAllTodos()
    {
        IEnumerable<Todo> todos = await _repo.GetTodos();
        return todos.Select(t => MapTodo2DTO(t)).ToList();
    }

    [HttpDelete(Name = nameof(DeleteAllTodos))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task DeleteAllTodos()
    {
        await _repo.DeleteTodos();
    }

    [HttpGet("{todoId}", Name = nameof(GetTodo))]
    public async Task<ActionResult<TodoDTO>> GetTodo(int todoId)
    {
        Todo? todo = await _repo.GetTodoById(todoId);
        if (todo is null)
        {
            return NotFound();
        }
        return MapTodo2DTO(todo);
    }

    [HttpPatch("{todoId}", Name = nameof(PatchTodo))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoDTO>> PatchTodo(int todoId, TodoDTO dto)
    {
        Todo? todo = await _repo.GetTodoById(todoId);
        if (todo is null)
        {
            return NotFound();
        }
        MapDTO2Todo(dto, todo);
        await _repo.UpdateTodo(todo);
        await _repo.SaveChanges();
        return MapTodo2DTO(todo);
    }

    [HttpDelete("{todoId}", Name = nameof(DeleteTodo))]
    public async Task<IActionResult> DeleteTodo(int todoId)
    {
        Todo? todo = await _repo.GetTodoById(todoId);
        if (todo is null)
        {
            return NotFound();
        }
        await _repo.DeleteTodo(todo);
        await _repo.SaveChanges();
        return Ok();
    }

    private TodoDTO MapTodo2DTO(Todo todo)
    {
        return new TodoDTO
        {
            Completed = todo.Completed,
            Order = todo.Order,
            Title = todo.Title,
            Url = Url.ActionLink(nameof(GetTodo), values: new { todoId = todo.TodoId }) ?? ""
        };
    }

    private Todo MapDTO2Todo(TodoDTO dto, Todo? todo = null)
    {
        todo ??= new Todo();

        if (dto.Title is not null)
        {
            todo.Title = dto.Title;
        }

        if (dto.Completed is not null)
        {
            todo.Completed = dto.Completed.Value;
        }

        if (dto.Order is not null)
        {
            todo.Order = dto.Order.Value;
        }

        return todo;
    }
}
