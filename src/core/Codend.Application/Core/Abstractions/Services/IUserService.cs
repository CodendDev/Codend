using Codend.Contracts.Responses;
using Codend.Domain.Entities;

namespace Codend.Application.Core.Abstractions.Services;

/// <summary>
/// Interface for user managment.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves users info with given ids.
    /// </summary>
    /// <param name="usersIds">Ids of the users whose info will be retrieved.</param>
    /// <returns>List of users info.</returns>
    public Task<IEnumerable<UserResponse>> GetUsersByIds(IEnumerable<UserId> usersIds);
}