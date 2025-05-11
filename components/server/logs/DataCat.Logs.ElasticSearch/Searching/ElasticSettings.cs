namespace DataCat.Logs.ElasticSearch.Searching;

public sealed class ElasticSettings
{
    public const string ConfigurationSection = "ElasticSearch";

    [Required, Url]
    public string ClusterUrl { get; set; } = null!;

    [Required, MinLength(1)]
    public string IndexPattern { get; set; } = null!;

    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    public bool EnableDebugLogging { get; set; }
}