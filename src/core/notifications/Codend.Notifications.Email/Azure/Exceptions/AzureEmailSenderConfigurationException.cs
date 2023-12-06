namespace Codend.Notifications.Email.Azure.Exceptions;

internal class AzureEmailSenderConfigurationException : AzureEmailConfigurationException
{
    public AzureEmailSenderConfigurationException()
        : base("Azure sender email is not valid.")
    {
    }
}