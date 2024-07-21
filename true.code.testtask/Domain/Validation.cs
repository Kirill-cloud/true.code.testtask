using @true.code.testtask.Domain.Models;

namespace @true.code.testtask.Domain;

public class ValidationResult
{
    private List<string> _errors = [];
    public bool IsSuccess => _errors.Count == 0;

    public string Error => string.Join(", ", _errors);

    public void AddError(string error)
    {
        _errors.Add(error);
    }
}

public static class Validation
{
    public static ValidationResult Validate(this User user)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(user.Name)) result.AddError("username must specified");

        return result;
    }

    public static ValidationResult Validate(this TodoItem todo)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(todo.Title)) result.AddError("title must specified");

        if (string.IsNullOrWhiteSpace(todo.Description)) result.AddError("description must specified");

        return result;
    }
}