namespace Codend.Notifications.Email.Azure.Exceptions;

internal abstract class AzureEmailConfigurationException : Exception
{
    protected AzureEmailConfigurationException(string message) : base(message)
    {
    }
}