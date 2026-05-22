using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace NinjaOne.Client;

public partial class NinjaOneClient
{
    /// <summary>
    /// Creates a <see cref="NinjaOneClient"/> authenticated with the given access token.
    /// </summary>
    /// <param name="baseUrl">The NinjaOne instance URL (e.g., https://app.ninjarmm.com)</param>
    /// <param name="accessToken">The OAuth2 Bearer access token</param>
    /// <param name="httpClient">Optional pre-configured HttpClient</param>
    public static NinjaOneClient Create(string baseUrl, string accessToken, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        var tokenProvider = new StaticAccessTokenProvider(accessToken);
        var authProvider = new BaseBearerTokenAuthenticationProvider(tokenProvider);
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient ?? new HttpClient())
        {
            BaseUrl = baseUrl.TrimEnd('/')
        };
        return new NinjaOneClient(adapter);
    }
}
