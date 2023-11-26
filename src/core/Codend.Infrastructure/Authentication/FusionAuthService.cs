using Codend.Application.Core.Abstractions.Authentication;
using Codend.Application.Core.Abstractions.Services;
using Codend.Application.Exceptions;
using Codend.Application.Users.Commands.UpdateUser;
using Codend.Contracts.Responses;
using Codend.Domain.Entities;
using FluentResults;
using io.fusionauth;
using io.fusionauth.domain;
using io.fusionauth.domain.api;
using io.fusionauth.domain.api.user;
using Microsoft.Extensions.Options;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Fusionauth implementation of <see cref="IAuthService"/>.
/// </summary>
public sealed class FusionAuthService : IAuthService, IUserService
{
    private readonly IFusionAuthAsyncClient _fusionAuthClient;
    private readonly Guid _appId;

    public FusionAuthService(IOptions<FusionauthConfiguration> configuration)
    {
        var fusionauthConfiguration = configuration.Value;
        _fusionAuthClient = new FusionAuthClient(
            fusionauthConfiguration.ApiKey,
            fusionauthConfiguration.ApiUrl,
            fusionauthConfiguration.TenantId);
        _appId = new Guid(fusionauthConfiguration.ApplicationId);
    }

    /// <inheritdoc />
    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest()
        {
            applicationId = _appId,
            loginId = email,
            password = password,
        };
        var response = await _fusionAuthClient.LoginAsync(loginRequest);

        if (response.WasSuccessful())
        {
            return Result.Ok(response.successResponse.token);
        }

        return response.statusCode switch
        {
            401 => throw new AuthenticationServiceException(response.ToString()),
            404 => Result.Fail(new AuthErrors.Login.InvalidEmailOrPassword()),
            _ => throw new AuthenticationServiceException(response.ToString())
        };
    }

    /// <inheritdoc />
    public async Task<Result<string>> RegisterAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        string imageUrl)
    {
        var newUser = new User()
        {
            active = true,
            email = email,
            password = password,
            firstName = firstName,
            lastName = lastName,
            imageUrl = imageUrl
        };
        var userRegistration = new UserRegistration()
        {
            applicationId = _appId,
            verified = true
        };
        var registration = new RegistrationRequest()
        {
            registration = userRegistration,
            skipVerification = true,
            user = newUser
        };

        var response = await _fusionAuthClient.RegisterAsync(null, registration);

        if (response.WasSuccessful())
        {
            return Result.Ok(response.successResponse.token);
        }

        if (response.statusCode != 400) throw new AuthenticationServiceException(response.ToString());

        // Checking if error response from fusionauth contains email field error, which means that email is already in use
        // or is not a valid email, but it should be covered by validation.
        if (response.errorResponse.fieldErrors.Any(err => err.Value.Any(e => e.code.Contains("email"))))
        {
            return Result.Fail(new AuthErrors.Register.EmailAlreadyExists());
        }

        // Same thing as above but for password field. 
        if (response.errorResponse.fieldErrors.Any(err => err.Value.Any(e => e.code.Contains("password"))))
        {
            return Result.Fail(new AuthErrors.Register.PasswordNotValid());
        }

        throw new AuthenticationServiceException(response.ToString());
    }

    /// <inheritdoc />
    public async Task<List<UserDetails>> GetUsersByIdsAsync(List<UserId> usersIds)
    {
        if (!usersIds.Any())
        {
            return new List<UserDetails>();
        }

        var response = await _fusionAuthClient
            .SearchUsersByIdsAsync(usersIds.Select(x => x.Value.ToString()).ToList());

        if (!response.WasSuccessful())
        {
            throw new AuthenticationServiceException(response.errorResponse.ToString());
        }

        var usersResponse = response.successResponse.users
            .Select(user => new UserDetails((Guid)user.id!, user.firstName, user.lastName, user.email, user.imageUrl))
            .ToList();

        return usersResponse;
    }

    /// <inheritdoc />
    public async Task<UserDetails> GetUserByIdAsync(UserId userId)
    {
        var response = await _fusionAuthClient.RetrieveUserAsync(userId.Value);

        if (!response.WasSuccessful())
        {
            throw new AuthenticationServiceException(response.errorResponse.ToString());
        }

        var user = response.successResponse.user;
        return new UserDetails(userId.Value, user.firstName, user.lastName, user.email, user.imageUrl);
    }

    /// <inheritdoc />
    public async Task<Result> UpdateUserAsync(UserId userId, UpdateUserCommand command)
    {
        var currentUserDetails = await GetUserByIdAsync(userId);

        var user = new User()
        {
            email = currentUserDetails.Email,
            firstName = command.FirstName,
            lastName = command.LastName,
            imageUrl = command.ImageUrl
        };

        var userRequest = new UserRequest()
        {
            user = user
        };

        var response = await _fusionAuthClient.UpdateUserAsync(userId.Value, userRequest);

        if (!response.WasSuccessful())
        {
            throw new AuthenticationServiceException(response.errorResponse.ToString());
        }

        return Result.Ok();
    }
}