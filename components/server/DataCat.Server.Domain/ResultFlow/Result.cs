namespace DataCat.Server.Domain.ResultFlow;

public class Result
{
    public Result()
    {
        Error = null;
    }

    public Result(ErrorInfo error)
    {
        Error = error;
    }

    public Result(Exception ex)
    {
        Error = new ErrorInfo(ex);
    }

    public ErrorInfo Error { get; protected set; }

    public bool IsFailure => Error != null;

    public bool IsSuccess => !IsFailure;

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

    public static Result Combine(params Result[] results)
    {
        foreach (Result result in results)
        {
            if (result == null)
            {
                return Fail("Result is null");
            }

            if (result.IsFailure)
            {
                return result;
            }
        }

        return Success();
    }
}
