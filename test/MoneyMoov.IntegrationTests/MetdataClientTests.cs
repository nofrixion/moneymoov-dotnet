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
    /// Tests that the expected error response is received when an attempt is made to call
    /// the whoami endpoint without an access token.
    /// </summary>
    [Fact]
    public async Task Whoami_No_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiAsync(string.Empty);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.Problem.IsEmpty);

        Logger.LogDebug(response.Problem.ToJson());
    }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the whoami endpoint without an invalid access token.
    /// </summary>
    [Fact]
    public async Task Whoami_Invalid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiAsync("xxx");

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.Problem.IsEmpty);

        Logger.LogDebug(response.Problem.ToJson());
    }


    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the whoami merchant endpoint without an access token.
    /// </summary>
    [Fact]
    public async Task Whoami_Merchant_No_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiMerchantAsync(string.Empty);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.Problem.IsEmpty);

        Logger.LogDebug(response.Problem.ToJson());
    }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the whoami merchant endpoint without an invalid access token.
    /// </summary>
    [Fact]
    public async Task Whoami_Merchant_Invalid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiMerchantAsync("xxx");

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.Problem.IsEmpty);

        Logger.LogDebug(response.Problem.ToJson());
    }

    /// <summary>
    /// Tests that the a success response is received when an attempt is made to call
    /// the whoami merchant endpoint without a valid access token.
    /// </summary>
    [Fact(Skip ="Need to generate a new non-expiring merchant token on dev.")]
    public async Task Whoami_Merchant_Valid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiMerchantAsync(SandboxMerchantAccessToken);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.False(response.Data.IsNone);
        Assert.True(response.Problem.IsEmpty);

        Logger.LogDebug(System.Text.Json.JsonSerializer.Serialize((Merchant)response.Data));
    }

    /// <summary>
    /// Tests that the a success response is received when an attempt is made to call
    /// the whoami user endpoint without an valid access token.
    /// </summary>
    [Fact(Skip ="Need to generate a new non-expiring user token on dev.")]
    public async Task Whoami_Valid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var metadataApiClient = new MetadataClient(MoneyMoovApiBaseUrl);

        var response = await metadataApiClient.WhoamiAsync(SandboxUserAccessToken);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.False(response.Data.IsNone);
        Assert.True(response.Problem.IsEmpty);

        Logger.LogDebug(System.Text.Json.JsonSerializer.Serialize((User)response.Data));
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
