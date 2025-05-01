namespace DataCat.Traces.Jaeger.Core;

public sealed class JaegerSettings
{
    public required string ServerUrl { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public JaegerAuthType AuthType { get; init; } = JaegerAuthType.None;
    public string? Username { get; init; }
    public string? Password { get; init; }
    public string? AuthToken { get; init; }

    public void ThrowIfIsInvalid()
    {
        switch (AuthType)
        {
            case JaegerAuthType.Basic when string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password):
                throw new ArgumentException("Username and Password are required for Basic authentication");

            case JaegerAuthType.Bearer when string.IsNullOrEmpty(AuthToken):
                throw new ArgumentException("AuthToken is required for Bearer authentication");

            case JaegerAuthType.None when !string.IsNullOrEmpty(Username) 
                                          || !string.IsNullOrEmpty(Password)
                                          || !string.IsNullOrEmpty(AuthToken):
                throw new ArgumentException("Authentication credentials provided but AuthType is None");
        }
    }
}