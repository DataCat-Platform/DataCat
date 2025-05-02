namespace DataCat.Caching.Redis;

public sealed record RedisSettings
{
    public const string SectionName = "DistributedCacheSettings";
    
    [Required]
    public string? ServerUrl { get; init; }
    
    public string InstanceName { get; init; } = "datacat_";
    
    [Range(0, int.MaxValue)]
    public int ConnectTimeout { get; init; } = 5000;
    
    public bool AbortOnConnectFail { get; init; }
}
