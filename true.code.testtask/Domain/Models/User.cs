namespace @true.code.testtask.Domain.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<TodoItem> TodoItems { get; set; }
}