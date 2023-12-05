using Azure;
using Azure.Communication.Email;
using Codend.Notifications.Email.Abstractions;
using Codend.Notifications.Email.Azure.Exceptions;
using Microsoft.Extensions.Configuration;

namespace Codend.Notifications.Email.Azure;

internal class AzureEmailService : IEmailService
{
    private readonly string _sender;
    private readonly EmailClient _client;

    public AzureEmailService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzureEmail");
        if (connectionString is null)
        {
            throw new AzureEmailConnectionStringException();
        }

        _client = new EmailClient(connectionString);

        var emailSection = configuration.GetSection("AzureEmail");
        var sender = emailSection.GetSection("Sender");
        _sender = sender.Value ?? throw new AzureEmailSenderConfigurationException();
    }

    public Task SendNotificationAsync(EmailNotification message, CancellationToken cancellationToken)
    {
        return _client.SendAsync(
            WaitUntil.Started,
            _sender,
            message.Receiver,
            message.Subject,
            message.Message,
            default,
            cancellationToken
        );
    }
}