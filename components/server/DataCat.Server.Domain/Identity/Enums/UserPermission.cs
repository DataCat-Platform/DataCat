namespace DataCat.Server.Domain.Identity.Enums;

public abstract class UserPermission(string name, int value)
    : SmartEnum<UserPermission, int>(name, value)
{
    public static readonly UserPermission ReadMetadata = new ReadMetadataPermission();

    private sealed class ReadMetadataPermission() : UserPermission("read:metadata", 1);

    public static IReadOnlyCollection<UserPermission> All =>
    [
        ReadMetadata
    ];
}