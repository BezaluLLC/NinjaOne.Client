# NinjaOne Client SDK for .NET

A .NET 10 SDK generated with [Kiota](https://github.com/microsoft/kiota) from the NinjaOne API v2 OpenAPI description. The SDK is published to GitHub Packages and includes a thin console host only to facilitate packaging; the SDK surface lives under the generated `Sdk` folder when built in CI.

## Table of contents
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Getting started](#getting-started)
- [Authentication](#authentication)
- [Examples](#examples)
- [Configuration](#configuration)
- [Regenerating the SDK](#regenerating-the-sdk)
- [CI/CD](#cicd)
- [Contributing](#contributing)
- [Troubleshooting](#troubleshooting)
- [License](#license)

## Prerequisites
- .NET 10 SDK (the project targets `net10.0`).
- Access to the NinjaOne API v2 and credentials (API keys/secret) as provided by NinjaOne.
- GitHub access token with `read:packages` scope to restore from GitHub Packages.

## Installation
1) Add the GitHub Packages feed:
   ```bash
   dotnet nuget add source "https://nuget.pkg.github.com/BezaluLLC/index.json" \
     --name BezaluLLC \
     --username YOUR_GITHUB_USERNAME \
     --password YOUR_TOKEN \
     --store-password-in-clear-text
   ```
2) Add the package to your project (package ID defaults to the project name, `NinjaOne.Client`):
   ```bash
   dotnet add package NinjaOne.Client
   ```

## Getting started
Create the client using Kiota’s request adapter and call the generated request builders. The SDK is generated into the `Sdk` folder at build time, exposing a strongly typed `NinjaOneClient` entry point.

```csharp
using System.Net.Http;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Serialization.Json;
using NinjaOne.Client;

// Example: API key style authentication header (adjust for your auth scheme)
var apiKey = Environment.GetEnvironmentVariable("NINJAONE_API_KEY")!;
var apiSecret = Environment.GetEnvironmentVariable("NINJAONE_API_SECRET")!;
var basicToken = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{apiKey}:{apiSecret}"));

var authProvider = new AnonymousAuthenticationProvider();
var adapter = new HttpClientRequestAdapter(
    authProvider,
    httpClient: new HttpClient(),
    serializationWriterFactory: new KiotaJsonSerializationWriterFactory(),
    parseNodeFactory: new KiotaJsonParseNodeFactory());

// Attach auth header globally
adapter.BaseHeaders.Add("Authorization", $"Basic {basicToken}");

var client = new NinjaOneClient(adapter);

// Replace with actual resource paths from the generated SDK (IntelliSense friendly)
var devices = await client.Devices.GetAsync();
Console.WriteLine($"Devices returned: {devices?.Value?.Count ?? 0}");
```

> Note: Resource names and request builder paths are generated from the OpenAPI description. Use IntelliSense/metadata in `Sdk/` to discover available operations, query parameters, and models.

## Authentication
The NinjaOne API currently uses Basic credentials derived from API key and secret. If your account uses a different scheme, implement an `IAuthenticationProvider` and pass it to the request adapter:

```csharp
public sealed class NinjaOneAuthProvider : IAuthenticationProvider
{
    private readonly string _apiKey;
    private readonly string _apiSecret;

    public NinjaOneAuthProvider(string apiKey, string apiSecret)
    {
        _apiKey = apiKey;
        _apiSecret = apiSecret;
    }

    public Task AuthenticateRequestAsync(RequestInformation request, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
    {
        var token = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_apiKey}:{_apiSecret}"));
        request.Headers.Authorization = new("Basic", token);
        return Task.CompletedTask;
    }
}
```

Swap `AnonymousAuthenticationProvider` in the sample above with an instance of your auth provider.

## Examples
- **List resources:**
  ```csharp
  var result = await client.Devices.GetAsync(q =>
  {
      q.QueryParameters.Top = 50;
      q.QueryParameters.Filter = "status eq 'active'";
  });
  ```
- **Create or update:**
  ```csharp
  var created = await client.Devices.PostAsync(new Device
  {
      Name = "example-device",
      // set other properties per model definitions
  });
  ```
- **Error handling:** Kiota throws `ApiException` on non-success responses. Inspect `StatusCode`, `ResponseHeaders`, and `ResponseBody` for details.

> The actual operations and models depend on the OpenAPI spec. Use IntelliSense or browse the generated `Sdk` folder for concrete names.

## Configuration
- **Base URL:** The adapter defaults to the server URL defined in the OpenAPI description (`https://app.ninjarmm.com`). Override with `adapter.BaseUrl = "https://<custom-endpoint>";` if needed.
- **HTTP pipeline:** Customize the underlying `HttpClient` (timeouts, proxy, retry handlers, logging) and pass it into `HttpClientRequestAdapter`.
- **Serialization:** JSON is enabled via `KiotaJsonSerializationWriterFactory`. Add XML/other formats by providing additional factories if the API supports them.

## Regenerating the SDK
Use the Kiota CLI (installed as a .NET global tool) to regenerate from the published OpenAPI document:

```bash
# Install (once)
dotnet tool install --global Microsoft.OpenApi.Kiota

# Regenerate
kiota generate \
  --language CSharp \
  --openapi https://app.ninjarmm.com/apidocs/NinjaRMM-API-v2.json \
  --class-name NinjaOneClient \
  --namespace-name NinjaOne.Client \
  --output ./Sdk \
  --clean-output \
  --ll warning
```

After regeneration, run `dotnet build` to validate, then `dotnet pack` to produce the NuGet package.

## CI/CD
- `.github/workflows/nuget_publish.yml` regenerates the SDK with Kiota on every `v*` tag, builds, packs, and pushes to GitHub Packages.
- To add README generation: Kiota currently does not emit README files automatically. Keep this README maintained by hand, or script a template step before `dotnet pack` if desired.
- Provide `NUGET_AUTH_TOKEN`/`GITHUB_TOKEN` secrets in the workflow for publishing.

## Contributing
1. Make changes/regenerate the SDK.
2. Run `dotnet build`.
3. Add/update tests if present.
4. Submit a PR. Tag a release (`vX.Y.Z`) to publish a package.

## Troubleshooting
- **401/403:** Verify API key/secret and ensure the Authorization header matches the expected scheme.
- **Serialization errors:** Ensure models align with the API response; regenerate if the upstream spec changed.
- **Missing operations:** The SDK mirrors the OpenAPI document. If an endpoint is absent, confirm it exists in `NinjaRMM-API-v2.json`.

## License
This project is licensed under the terms of the LICENSE.txt in this repository.
