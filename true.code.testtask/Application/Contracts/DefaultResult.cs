namespace @true.code.testtask.Application.Contracts;

public class DefaultResult
{
    public bool IsSuccess { get; set; }
    public string Error { get; set; }

    public static DefaultResult CreateError(string error)
    {
        return new DefaultResult { Error = error, IsSuccess = false };
    }

    public static DefaultResult CreateSuccess()
    {
        return new DefaultResult { IsSuccess = true };
    }
}

public class DefaultResult<T>
{
    private List<string> _errors = [];
    public T Result { get; set; }

    public bool IsSuccess => _errors.Count == 0;

    public string Error => string.Join(", ", _errors);

    public static DefaultResult<T> CreateError(string error)
    {
        return new DefaultResult<T> { _errors = [error] };
    }

    public static DefaultResult<T> CreateSuccess(T result)
    {
        return new DefaultResult<T> { Result = result };
    }
}