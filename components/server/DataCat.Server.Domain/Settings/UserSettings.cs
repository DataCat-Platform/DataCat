namespace DataCat.Server.Domain.Settings;

public class UserSettings
{
    private UserSettings(
        Guid userId,
        bool isDarkThemeEnabled,
        string? timeZone,
        bool isNotificationsEnabled,
        DateTime createdAt,
        DateTime updatedAt)
    {
        UserId = userId;
        IsDarkThemeEnabled = isDarkThemeEnabled;
        TimeZone = timeZone;
        IsNotificationsEnabled = isNotificationsEnabled;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public Guid UserId { get; private set; }

    public bool IsDarkThemeEnabled { get; private set; }

    public string? TimeZone { get; private set; }

    public bool IsNotificationsEnabled { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<UserSettings> Create(
        Guid userId,
        bool isDarkThemeEnabled,
        string? timeZone,
        bool isNotificationsEnabled,
        DateTime createdAt,
        DateTime updatedAt)
    {
        if (userId == Guid.Empty)
        {
            return Result.Fail<UserSettings>("UserId cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(timeZone))
        {
            return Result.Fail<UserSettings>("TimeZone cannot be null or empty.");
        }

        return Result.Success(new UserSettings(
            userId,
            isDarkThemeEnabled,
            timeZone,
            isNotificationsEnabled,
            createdAt,
            updatedAt));
    }

    public void UpdateSettings(
        bool? isDarkThemeEnabled = null,
        string? timeZone = null,
        bool? isNotificationsEnabled = null)
    {
        if (isDarkThemeEnabled.HasValue)
        {
            IsDarkThemeEnabled = isDarkThemeEnabled.Value;
        }

        if (!string.IsNullOrWhiteSpace(timeZone))
        {
            TimeZone = timeZone;
        }

        if (isNotificationsEnabled.HasValue)
        {
            IsNotificationsEnabled = isNotificationsEnabled.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }
}