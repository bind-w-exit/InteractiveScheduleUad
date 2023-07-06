namespace InteractiveScheduleUad.Api;

public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public Exception? Exception { get; }

    private Result(T? value, bool isSuccess, Exception? errorMessage)
    {
        Value = value;
        IsSuccess = isSuccess;
        Exception = errorMessage;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, null);
    }

    public static Result<T> Failure(Exception errorMessage)
    {
        return new Result<T>(default, false, errorMessage);
    }
}