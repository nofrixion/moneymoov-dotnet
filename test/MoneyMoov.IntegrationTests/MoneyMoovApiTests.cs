//-----------------------------------------------------------------------------
// Filename: MoneyMoovApiTests.cs
//
// Description: Integrations tests for the MoneyMoovApi which supplies clients
// for the MoneyMoov API end points.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.IntegrationTests;
using NoFrixion.MoneyMoov.Models;
using Xunit.Abstractions;

namespace MoneyMoov.IntegrationTests;

[Trait("Category", "integration")]
public class MoneyMoovApiTests : MoneyMoovTestBase<MoneyMoovApiTests>
{
    public MoneyMoovApiTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public void Get_ConfigSettings_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");
        Logger.LogDebug($"MoneyMoov API Base URL={base.MoneyMoovApiBaseUrl}.");
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the production cluster.
    /// </summary>
    [Fact]
    public async Task Get_Version_Production_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovApi = new MoneyMoovApi(HttpClientFactory);

        var response = await moneyMoovApi.MetadataClient().GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the sandbox cluster.
    /// </summary>
    [Fact]
    public async Task Get_Version_Sandbox_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovApi = new MoneyMoovApi(HttpClientFactory);

        var response = await moneyMoovApi.MetadataClient().GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the API under test.
    /// </summary>
    [Fact]
    public async Task Get_Version_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovApi = new MoneyMoovApi(HttpClientFactory);

        var response = await moneyMoovApi.MetadataClient().GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }
}