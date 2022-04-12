namespace ApiTodoBackend.Models;

public class Todo
{
    public int TodoId { get; set; }

    public string Title { get; set; } = "";

    public bool Completed { get; set; }

    public int Order { get; set; }
}
