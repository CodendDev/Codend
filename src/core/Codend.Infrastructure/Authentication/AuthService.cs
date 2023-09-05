using Codend.Application.Core.Abstractions.Authentication;
using io.fusionauth;
using io.fusionauth.domain.api;
using Microsoft.Extensions.Options;

namespace Codend.Infrastructure.Authentication;

public sealed class AuthService : IAuthService
{
    private IFusionAuthAsyncClient _fusionAuthClient;
    private FusionauthConfiguration _fusionauthConfiguration;

    public AuthService(IOptions<FusionauthConfiguration> configuration)
    {
        _fusionauthConfiguration = configuration.Value;
        _fusionAuthClient = new FusionAuthClient(
            _fusionauthConfiguration.ApiKey,
            _fusionauthConfiguration.ApiUrl,
            _fusionauthConfiguration.TenantId);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var loginRequest = new LoginRequest()
        {
            applicationId = new Guid(_fusionauthConfiguration.ApplicationId),
            loginId = email,
            password = password
        };
        var response = await _fusionAuthClient.LoginAsync(loginRequest);

        if (!response.WasSuccessful())
        {
            throw new Exception("Cannot login");
        }

        return response.successResponse.token;
    }
}