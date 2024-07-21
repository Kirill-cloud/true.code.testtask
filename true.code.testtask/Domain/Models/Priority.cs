namespace @true.code.testtask.Domain.Models;

public class Priority
{
    public int Id { get; set; }
    public PriorityLevel Level { get; set; }
    public virtual List<TodoItem> ToDoItems { get; set; } = [];
}