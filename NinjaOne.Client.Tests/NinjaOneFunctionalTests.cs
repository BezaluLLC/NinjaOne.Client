namespace NinjaOne.Client.Tests;

/// <summary>
/// Functional tests that call the real NinjaOne API.
/// These require valid OAuth credentials configured via user-secrets or environment variables.
/// They are skipped automatically when credentials are not present.
/// 
/// Setup:
///   dotnet user-secrets --id ninjaone-client-tests set "NinjaOne:ClientId" "your-client-id"
///   dotnet user-secrets --id ninjaone-client-tests set "NinjaOne:ClientSecret" "your-secret"
///
/// Or via environment variables:
///   NINJAONE_NinjaOne__ClientId=your-client-id
///   NINJAONE_NinjaOne__ClientSecret=your-secret
/// </summary>
public class NinjaOneFunctionalTests : IClassFixture<NinjaOneFixture>
{
    private readonly NinjaOneFixture _fixture;

    public NinjaOneFunctionalTests(NinjaOneFixture fixture)
    {
        _fixture = fixture;
    }


    [Fact]
    public async Task WhenAuthenticatedThenCanGetOrganizations()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Organizations.GetAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenAuthenticatedThenCanGetDevices()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Devices.GetAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenAuthenticatedThenCanGetAlerts()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Alerts.GetAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenAuthenticatedThenCanGetUsers()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Users.GetAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenAuthenticatedThenCanGetPolicies()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Policies.GetAsync();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenAuthenticatedThenCanGetGroups()
    {
        SkipIfNotConfigured();
        var result = await _fixture.Client.V2.Groups.GetAsync();
        Assert.NotNull(result);
    }

    private void SkipIfNotConfigured()
    {
        if (!_fixture.IsConfigured)
            Assert.Skip("NinjaOne credentials not configured.");
    }
}
