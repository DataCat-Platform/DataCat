namespace DataCat.Server.Application.Exceptions;

public class ForbiddenException : Exception
{
    public int StatusCode { get; }
    public string MessageDetails { get; }

    public ForbiddenException(string message = "You do not have permission to perform this action.")
        : base(message)
    {
        StatusCode = 403;
    }

    public ForbiddenException(string message, string messageDetails)
        : base(message)
    {
        StatusCode = 403;
        MessageDetails = messageDetails;
    }
}