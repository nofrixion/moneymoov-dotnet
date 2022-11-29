//-----------------------------------------------------------------------------
// Filename: MoneyMoovServiceTests.cs
//
// Description: Integrations tests for the MoneyMoov service which calls
// the MoneyMoov API end points.
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
using NoFrixion.MoneyMoov.Services;
using Xunit.Abstractions;

namespace MoneyMoov.IntegrationTests;

public class MoneyMoovServiceTests : MoneyMoovTestBase<MoneyMoovServiceTests>
{
    public MoneyMoovServiceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that the get version method can be correctly called on the production cluster.
    /// </summary>
    [Fact]
    public async Task Get_Version_Production_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovService = new MoneyMoovService(
            LoggerFactory.CreateLogger<MoneyMoovService>(),
            Configuration,
            HttpClientFactory);

        var response = await moneyMoovService.VersionAsync();

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

        var moneyMoovService = new MoneyMoovService(
            LoggerFactory.CreateLogger<MoneyMoovService>(),
            Configuration,
            HttpClientFactory);

        moneyMoovService.SetBaseUrl(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await moneyMoovService.VersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }
}