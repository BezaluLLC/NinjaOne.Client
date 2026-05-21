using Microsoft.Kiota.Abstractions;
using Moq;

namespace NinjaOne.Client.Tests;

public class NinjaOneClientTests
{
    private readonly Mock<IRequestAdapter> _mockAdapter;
    private readonly NinjaOneClient _client;

    public NinjaOneClientTests()
    {
        _mockAdapter = new Mock<IRequestAdapter>();
        _mockAdapter.Setup(a => a.BaseUrl).Returns("https://app.ninjarmm.com");
        _client = new NinjaOneClient(_mockAdapter.Object);
    }

    [Fact]
    public void WhenClientIsInstantiatedThenItIsNotNull()
    {
        Assert.NotNull(_client);
    }

    [Fact]
    public void WhenAccessingV2ThenRequestBuilderIsReturned()
    {
        var v2 = _client.V2;

        Assert.NotNull(v2);
    }

    [Fact]
    public void WhenAccessingOrganizationsThenRequestBuilderIsReturned()
    {
        var organizations = _client.V2.Organizations;

        Assert.NotNull(organizations);
    }

    [Fact]
    public void WhenAccessingDevicesThenRequestBuilderIsReturned()
    {
        var devices = _client.V2.Devices;

        Assert.NotNull(devices);
    }

    [Fact]
    public void WhenAccessingAlertsThenRequestBuilderIsReturned()
    {
        var alerts = _client.V2.Alerts;

        Assert.NotNull(alerts);
    }

    [Fact]
    public void WhenAccessingDevicesSearchThenRequestBuilderIsReturned()
    {
        var search = _client.V2.Devices.Search;

        Assert.NotNull(search);
    }

    [Fact]
    public void WhenAccessingTicketingThenRequestBuilderIsReturned()
    {
        var ticketing = _client.V2.Ticketing;

        Assert.NotNull(ticketing);
    }

    [Fact]
    public void WhenAccessingUsersThenRequestBuilderIsReturned()
    {
        var users = _client.V2.Users;

        Assert.NotNull(users);
    }

    [Fact]
    public void WhenAccessingGroupsThenRequestBuilderIsReturned()
    {
        var groups = _client.V2.Groups;

        Assert.NotNull(groups);
    }

    [Fact]
    public void WhenAccessingPoliciesthenRequestBuilderIsReturned()
    {
        var policies = _client.V2.Policies;

        Assert.NotNull(policies);
    }

    [Fact]
    public void WhenAccessingQueriesthenRequestBuilderIsReturned()
    {
        var queries = _client.V2.Queries;

        Assert.NotNull(queries);
    }

    [Fact]
    public void WhenClientIsCreatedWithNullAdapterThenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => new NinjaOneClient(null!));
    }

    [Fact]
    public void WhenAccessingMultipleBuildersSequentiallyThenEachIsIndependent()
    {
        var orgs = _client.V2.Organizations;
        var devices = _client.V2.Devices;
        var alerts = _client.V2.Alerts;

        Assert.NotSame(orgs, devices);
        Assert.NotSame(devices, alerts);
    }
}
