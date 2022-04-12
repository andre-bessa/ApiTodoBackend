using System.ComponentModel.DataAnnotations;

namespace ApiTodoBackend.Models;

public class TodoDTO
{
    public string? Title { get; set; }

    public bool? Completed { get; set; }

    [Range(0, int.MaxValue)]
    public int? Order { get; set; }

    public string? Url { get; set; }
}
