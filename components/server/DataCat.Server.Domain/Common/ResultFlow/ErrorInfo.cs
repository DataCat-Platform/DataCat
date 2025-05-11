namespace DataCat.Server.Domain.Common.ResultFlow;

public class ErrorInfo
{
    public ErrorInfo(string error)
    {
        ErrorMessage = error;
        Exception = null;
    }

    public ErrorInfo(Exception ex)
    {
        ErrorMessage = ex.Message;
        Exception = ex;
    }

    public ErrorInfo(string errorCode, string error)
    {
        ErrorCode = errorCode;
        ErrorMessage = error;
    }

    public ErrorInfo(Exception ex, string errorMessage)
    {
        ErrorMessage = errorMessage;
        Exception = ex;
    }

    public string ErrorMessage { get; private set; }

    public string ErrorCode { get; private set; }

    public Exception Exception { get; private set; }

    public ErrorInfo AppendErrorMessage(string message)
    {
        ErrorMessage = ErrorMessage + "\n" + message;
        return this;
    }

    public ErrorInfo PrependErrorMessage(string message)
    {
        ErrorMessage = message + "\n" + ErrorMessage;
        return this;
    }
}
