using Microsoft.Extensions.Configuration;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;

namespace NinjaOne.Client.Tests;

public class NinjaOneFixture : IAsyncLifetime
{
    public NinjaOneClient Client { get; private set; } = null!;
    public bool IsConfigured { get; private set; }

    private string _accessToken = string.Empty;

    public async ValueTask InitializeAsync()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets("ninjaone-client-tests")
            .AddEnvironmentVariables("NINJAONE_")
            .Build();

        var section = config.GetSection("NinjaOne");
        var clientId = section["ClientId"];
        var clientSecret = section["ClientSecret"];
        var tokenUrl = section["TokenUrl"] ?? "https://app.ninjarmm.com/oauth/token";
        var baseUrl = section["BaseUrl"] ?? "https://app.ninjarmm.com";

        var scope = section["Scope"] ?? "monitoring";

        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            IsConfigured = false;
            return;
        }

        IsConfigured = true;
        _accessToken = await AcquireTokenAsync(tokenUrl, clientId, clientSecret, scope);

        var authProvider = new BaseBearerTokenAuthenticationProvider(
            new StaticAccessTokenProvider(_accessToken));
        var adapter = new HttpClientRequestAdapter(authProvider)
        {
            BaseUrl = baseUrl
        };

        Client = new NinjaOneClient(adapter);
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private static async Task<string> AcquireTokenAsync(string tokenUrl, string clientId, string clientSecret, string scope)
    {
        using var http = new HttpClient();
        var pairs = new List<KeyValuePair<string, string>>
        {
            new("grant_type", "client_credentials"),
            new("client_id", clientId),
            new("client_secret", clientSecret),
            new("scope", scope),
        };

        var content = new FormUrlEncodedContent(pairs);

        var response = await http.PostAsync(tokenUrl, content);
        var json = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Token request failed ({response.StatusCode}). Response: {json}");
        }
        var doc = System.Text.Json.JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("access_token").GetString()
            ?? throw new InvalidOperationException("No access_token in response.");
    }

    private sealed class StaticAccessTokenProvider(string token) : IAccessTokenProvider
    {
        public AllowedHostsValidator AllowedHostsValidator { get; } = new();

        public Task<string> GetAuthorizationTokenAsync(
            Uri uri,
            Dictionary<string, object>? additionalAuthenticationContext = null,
            CancellationToken cancellationToken = default)
            => Task.FromResult(token);
    }
}
