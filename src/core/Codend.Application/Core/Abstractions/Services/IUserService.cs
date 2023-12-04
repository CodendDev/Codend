using Codend.Application.Users.Commands.UpdateUser;
using Codend.Contracts.Responses;
using Codend.Domain.Entities;
using FluentResults;

namespace Codend.Application.Core.Abstractions.Services;

/// <summary>
/// Interface for user management.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves users info with given ids.
    /// </summary>
    /// <param name="usersIds">Ids of the users whose info will be retrieved.</param>
    /// <returns>List of users info.</returns>
    public Task<List<UserDetails>> GetUsersByIdsAsync(List<UserId> usersIds);

    /// <summary>
    /// Retrieves details of user with given id.
    /// </summary>
    /// <param name="userId">Id of the user whose details will be returned.</param>
    /// <returns>User details as <see cref="UserDetails"/></returns>
    public Task<UserDetails> GetUserByIdAsync(UserId userId);

    /// <summary>
    /// Retrieves details of user with given email.
    /// </summary>
    /// <param name="email">Email og the user whose details will be returned.</param>
    /// <returns>User details as <see cref="UserDetails"/></returns>
    public Task<UserDetails?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Updates user data.
    /// </summary>
    /// <param name="userId">Id of the user which will be updated.</param>
    /// <param name="command">Command containing new user data.</param>
    /// <returns>Result of the update.</returns>
    public Task<Result> UpdateUserAsync(UserId userId, UpdateUserCommand command);
}