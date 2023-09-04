using Codend.Application.Core.Abstractions.Authentication;
using Codend.Domain.Entities;

namespace Codend.Infrastructure.Authentication;

public class UserIdentityProvider : IUserIdentityProvider
{
    public UserId UserId { get; }
}