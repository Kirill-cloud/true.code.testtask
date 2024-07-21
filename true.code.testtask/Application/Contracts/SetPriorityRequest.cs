using @true.code.testtask.Domain.Models;

namespace @true.code.testtask.Application.Contracts;

public class SetPriorityRequest
{
    public int TodoId { get; set; }

    public PriorityLevel PriorityLevel { get; set; }
}