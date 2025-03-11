namespace DataCat.Server.HttpModels.Requests.Security;

public class SecretModel
{
    public required string Key { get; init; }
    
    public required string Value { get; init; }
}