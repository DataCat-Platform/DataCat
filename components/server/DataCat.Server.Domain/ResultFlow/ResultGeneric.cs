namespace DataCat.Server.Domain.ResultFlow;

public class Result<T> : Result
{
    #region constructors

    public Result(T value)
    {
        Value = value;
        Error = null;
    }

    public Result(ErrorInfo error)
    {
        Value = default;
        Error = error;
    }

    public Result(Exception ex)
    {
        Value = default;
        Error = new ErrorInfo(ex);
    }

    #endregion constructors

    public T Value { get; private set; }

    public static Result<T> Success(T value) => new Result<T>(value);

    public static new Result<T> Fail(ErrorInfo error) => new Result<T>(error);

    public static new Result<T> Fail(string message) => new Result<T>(new ErrorInfo(message));

    public static new Result<T> Fail(Exception ex) => new Result<T>(new ErrorInfo(ex));

    public static new Result<T> Fail(Exception ex, string message) => new Result<T>(new ErrorInfo(ex, message));
}
