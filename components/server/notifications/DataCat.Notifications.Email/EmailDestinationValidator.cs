namespace DataCat.Notifications.Email;

public static class EmailDestinationValidator
{
    public static bool IsEmailDestination(NotificationDestination? destination)
    {
        if (destination is null)
        {
            return false;
        }

        return string.Compare(destination.Name, EmailConstants.Email, 
                   StringComparison.InvariantCultureIgnoreCase) == 0;
    }
}