//-----------------------------------------------------------------------------
// Filename: MetadataClientTest.cs
//
// Description: Integrations tests for the MoneyMoov Metadata API client.
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
public class MetadataClientTests : MoneyMoovTestBase<MetadataClientTests>
{
    public MetadataClientTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that the get version method can be correctly called on the production cluster.
    /// </summary>
    [Fact]
    public async Task Get_Version_Production_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient();

        var response = await metadataApiClient.GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the sandbox cluster when using the 
    /// MoneyMoov client.
    /// </summary>
    [Fact]
    public async Task Get_Version_MoneyMoovClient_Production_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovClient = new MoneyMoovClient();

        Assert.Equal(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL, moneyMoovClient.GetBaseUrl()?.ToString());

        var response = await moneyMoovClient.MetadataClient().GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

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

        var metadataApiClient = new MetadataClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await metadataApiClient.GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the sandbox cluster when using the 
    /// MoneyMoov client.
    /// </summary>
    [Fact]
    public async Task Get_Version_MoneyMoovClient_Sandbox_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var moneyMoovClient = new MoneyMoovClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        Assert.Equal(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL, moneyMoovClient.GetBaseUrl()?.ToString());

        var response = await moneyMoovClient.MetadataClient().GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the get version method can be correctly called on the app settings configured API URL.
    /// </summary>
    [Fact]
    public async Task Get_Version_Custom_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.GetVersionAsync();

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        var version = (NoFrixionVersion)response.Data;

        Logger.LogDebug(version.ToString());
    }

    /// <summary>
    /// Tests that the echo method can be correctly called on the sandbox cluster when
    /// using form URL encoding.
    /// </summary>
    [Fact]
    public async Task Echo_Form_Encoding_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.EchoAsync("Alice", "Hello World");

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        Logger.LogDebug((string)response.Data);
    }

    /// <summary>
    /// Tests that the echo method can be correctly called on the sandbox cluster when using JSON
    /// encoding.
    /// </summary>
    [Fact]
    public async Task Echo_Json_Encoding_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.EchoJsonAsync("Alice", "Hello World");

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.Problem.IsEmpty);

        response.Data.Match(
            Some: x =>
            {
                var echoResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string,string>>(x.ToString());
                Assert.NotNull(echoResult["name"]);
                Assert.NotNull(echoResult["message"]);

                Logger.LogDebug($"Echo Result: name={echoResult["name"]}, message={echoResult["message"]}.");
            },
            None: () => throw new ApplicationException("Should not be here")
        ); 
    }
}
