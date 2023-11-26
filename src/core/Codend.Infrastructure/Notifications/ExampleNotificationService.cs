using Codend.Application.Core.Abstractions.Notifications;
using Codend.Domain.Core.Abstractions;
using Microsoft.Extensions.Logging;

namespace Codend.Infrastructure.Notifications;

public record LogNotification(IUser User, string Message);

public class ExampleNotificationService : IUserNotificationService
{
    private readonly ILogger<ExampleNotificationService> _logger;

    public ExampleNotificationService(ILogger<ExampleNotificationService> logger)
    {
        _logger = logger;
    }

    public string ServiceName => "Logger";

    public Task SendNotification(IUser user, object message) =>
        message switch
        {
            LogNotification email => SendNotification(user, email),
            string messageString => SendNotification(new LogNotification(user, messageString)),
            _ => Task.CompletedTask
        };

    private Task SendNotification(LogNotification message)
    {
        _logger.LogInformation(
            "{service} - {user}: {message}",
            ServiceName, message.User.UserId, message.Message
        );
        return Task.CompletedTask;
    }
}