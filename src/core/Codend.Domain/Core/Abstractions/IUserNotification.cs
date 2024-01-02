namespace Codend.Domain.Core.Abstractions;

public interface IUserNotification : IDomainEvent
{
    IUser Receiver { get; }
}