using @true.code.testtask.Domain.Models;

namespace @true.code.testtask.Application.Contracts;

public class TodoFilter
{
    public int? UserId { get; set; }

    public PriorityLevel? PriorityLevel { get; set; }

    public bool? IsCompleted { get; set; }
}