namespace @true.code.testtask.Domain.Models;

public class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime DueDate { get; set; }

    public virtual Priority Priority { get; set; }

    public int? PriorityId { get; set; }

    public virtual User User { get; set; }

    public int? UserId { get; set; }
}