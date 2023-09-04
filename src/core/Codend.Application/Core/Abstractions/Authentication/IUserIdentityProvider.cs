using Codend.Domain.Entities;

namespace Codend.Application.Core.Abstractions.Authentication;

public interface IUserIdentityProvider
{
    UserId UserId { get; }
}