namespace DataCat.Server.Domain.Exceptions;

public class DatabaseMappingException : Exception
{
    public DatabaseMappingException(Type @class) 
        : base($"Cannot map the class: {@class} to database.")
    {
    }
    
    public DatabaseMappingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}