namespace Codend.Notifications.Email.Azure.Exceptions;

internal class AzureEmailConnectionStringException : AzureEmailConfigurationException
{
    public AzureEmailConnectionStringException()
        : base("Azure email connection string is not valid.")
    {
    }
}