namespace Codend.Contracts.Authentication;

/// <summary>
/// JWT Token class holder.
/// </summary>
public sealed class TokenResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenResponse"/> class.
    /// </summary>
    public TokenResponse(string accessToken) => AccessToken = accessToken;

    /// <summary>
    /// API Access token.
    /// </summary>
    public string AccessToken { get; private set; }
}