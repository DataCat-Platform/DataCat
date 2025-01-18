namespace DataCat.Server.Domain.Common.ResultFlow;

public class Result<T> : Result
{
    #region constructors

    public Result(T value)
    {
        Value = value;
        Errors = null;
    }

    public Result(ErrorInfo error)
    {
        Value = default!;
        Errors = [error];
    }

    public Result(Exception ex)
    {
        Value = default!;
        Errors = [new ErrorInfo(ex)];
    }

    #endregion constructors

    public T Value { get; private set; }
    
    public new Result<T> Append(string message)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(message));
        return this;
    }
    
    public new Result<T> Append(Exception ex)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(ex));
        return this;
    }
    
    public new Result<T> Append(Exception ex, string message)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(ex, message));
        return this;
    }

    public static Result<T> Success(T value) => new Result<T>(value);

    public new static Result<T> Fail(ErrorInfo error) => new Result<T>(error);

    public new static Result<T> Fail(string message) => new Result<T>(new ErrorInfo(message));

    public new static Result<T> Fail(Exception ex) => new Result<T>(new ErrorInfo(ex));

    public new static Result<T> Fail(Exception ex, string message) => new Result<T>(new ErrorInfo(ex, message));
}
