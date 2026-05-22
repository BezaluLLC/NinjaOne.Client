using Microsoft.Kiota.Abstractions.Authentication;

namespace NinjaOne.Client;

/// <summary>
/// Provides a static access token for Bearer authentication.
/// </summary>
public sealed class StaticAccessTokenProvider(string accessToken) : IAccessTokenProvider
{
    /// <inheritdoc/>
    public AllowedHostsValidator AllowedHostsValidator { get; } = new();

    /// <inheritdoc/>
    public Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
        => Task.FromResult(accessToken);
}
