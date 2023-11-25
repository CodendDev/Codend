namespace Codend.Domain.Core.Abstractions;

public interface IUserNotification : IDomainEvent
{
    IUser User { get; }
}