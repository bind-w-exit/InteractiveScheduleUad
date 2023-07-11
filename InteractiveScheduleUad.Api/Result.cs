namespace InteractiveScheduleUad.Api;

public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public Exception? Exception { get; }

    public Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Exception = null;
    }

    public Result(Exception e)
    {
        IsSuccess = false;
        Exception = e;
        Value = default;
    }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Exception exception) => new(exception);

    public TResult Match<TResult>(
        Func<T, TResult> success,
        Func<Exception, TResult> faulted) =>
        IsSuccess ? success(Value!) : faulted(Exception!);
}