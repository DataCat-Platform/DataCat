namespace DataCat.Metrics.Prometheus.Core;

public sealed class PrometheusSettings
{
    public required string ServerUrl { get; init; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PrometheusAuthType AuthType { get; init; } = PrometheusAuthType.None;

    public string? Username { get; init; }
    public string? Password { get; init; }
    public string? AuthToken { get; init; }

    public void ThrowIfIsInvalid()
    {
        switch (AuthType)
        {
            case PrometheusAuthType.Basic when string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password):
                throw new ArgumentException("Username and Password are required for Basic authentication");

            case PrometheusAuthType.Bearer when string.IsNullOrEmpty(AuthToken):
                throw new ArgumentException("AuthToken is required for Bearer authentication");

            case PrometheusAuthType.None when !string.IsNullOrEmpty(Username) 
                                              || !string.IsNullOrEmpty(Password)
                                              || !string.IsNullOrEmpty(AuthToken):
                throw new ArgumentException("Authentication credentials provided but AuthType is None");
        }
    }
}