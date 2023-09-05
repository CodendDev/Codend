using Microsoft.Extensions.Configuration;

namespace Codend.Infrastructure.Authentication;

/// <summary>
/// Fusionauth settings class containing it's configuration.
/// </summary>
public class FusionauthConfiguration
{
    /// <summary>
    /// Fusionauth secret Api key.
    /// </summary>
    [ConfigurationKeyName("ApiKey")]
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// Fusionauth api url.
    /// </summary>
    [ConfigurationKeyName("ApiUrl")]
    public string ApiUrl { get; set; } = null!;

    /// <summary>
    /// This .Net core application fusionauth id.
    /// </summary>
    [ConfigurationKeyName("ApplicationId")]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Fusionauth as issuer url.
    /// </summary>
    [ConfigurationKeyName("Issuer")]
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Fusionauth access token signing key.
    /// </summary>
    [ConfigurationKeyName("SigningKey")]
    public string SigningKey { get; set; } = null!;

    /// <summary>
    /// Tenant id.
    /// </summary>
    [ConfigurationKeyName("TenantId")]
    public string TenantId { get; set; } = null!;
}