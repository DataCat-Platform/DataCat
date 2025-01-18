namespace DataCat.Server.Domain.Common.ResultFlow;

public class Result
{
    public Result()
    {
        Errors = null;
    }

    public Result(ErrorInfo error)
    {
        Errors = [error];
    }

    public Result(Exception ex)
    {
        Errors = [new ErrorInfo(ex)];
    }

    public List<ErrorInfo>? Errors { get; protected set; }

    public bool IsFailure => Errors != null;

    public bool IsSuccess => !IsFailure;
    
    public Result Append(string message)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(message));
        return this;
    }
    
    public Result Append(Exception ex)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(ex));
        return this;
    }
    
    public Result Append(Exception ex, string message)
    {
        Errors ??= [];   
        Errors.Add(new ErrorInfo(ex, message));
        return this;
    }

    public static Result Success() => new Result();

    public static Result Fail(ErrorInfo error) => new Result(error);

    public static Result Fail(string message) => new Result(new ErrorInfo(message));

    public static Result Fail(Exception ex) => new Result(new ErrorInfo(ex));

    public static Result Fail(Exception ex, string message) => new Result(new ErrorInfo(ex, message));

    public static Result<T> Success<T>(T value) => new Result<T>(value);

    public static Result<T> Fail<T>(ErrorInfo error) => new Result<T>(error);

    public static Result<T> Fail<T>(string message) => new Result<T>(new ErrorInfo(message));

    public static Result<T> Fail<T>(Exception ex) => new Result<T>(new ErrorInfo(ex));

    public static Result<T> Fail<T>(Exception ex, string message) => new Result<T>(new ErrorInfo(ex, message));
}
