namespace DataCat.Server.Domain.Core.Enums;

public abstract class AlertStatus(string name, int value)
    : SmartEnum<AlertStatus, int>(name, value)
{
    public static readonly AlertStatus Fire = new FireAlertStatus();
    public static readonly AlertStatus Error = new ErrorAlertStatus();
    public static readonly AlertStatus Muted = new MuteAlertStatus();
    public static readonly AlertStatus Ok = new OkAlertStatus();

    private sealed class FireAlertStatus() : AlertStatus("Firing", 1);
    
    private sealed class MuteAlertStatus() : AlertStatus("Muted", 2);
    
    private sealed class OkAlertStatus() : AlertStatus("Ok", 3);
    private sealed class ErrorAlertStatus() : AlertStatus("Error", 4);
}