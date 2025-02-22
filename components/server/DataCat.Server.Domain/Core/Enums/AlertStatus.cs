namespace DataCat.Server.Domain.Core.Enums;

public abstract class AlertStatus(string name, int value)
    : SmartEnum<AlertStatus, int>(name, value)
{
    public static readonly AlertStatus Fire = new FireAlertStatus();
    public static readonly AlertStatus Muted = new MuteAlertStatus();
    public static readonly AlertStatus InActive = new InActiveAlertStatus();

    private sealed class FireAlertStatus() : AlertStatus("Fire", 1);
    
    private sealed class MuteAlertStatus() : AlertStatus("Muted", 2);
    
    private sealed class InActiveAlertStatus() : AlertStatus("InActive", 3);
}