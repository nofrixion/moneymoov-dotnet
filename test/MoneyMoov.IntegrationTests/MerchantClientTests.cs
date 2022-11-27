//-----------------------------------------------------------------------------
// Filename: MerchantClientTest.cs
//
// Description: Integrations tests for the MoneyMoov Merchant API client.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 27 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.IntegrationTests;
using Xunit.Abstractions;

namespace MoneyMoov.IntegrationTests;

public class MerchantClientTests : MoneyMoovTestBase<MerchantClientTests>
{
    public MerchantClientTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the get user roles endpoint without an access token.
    /// </summary>
    [Fact]
    public async Task Get_UserRoles_Sandbox_No_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var httpClient = HttpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);
        var apiClient = new MoneyMoovApiClient(httpClient);
        var merchantApiClient = new MerchantClient(apiClient);

        var response = await merchantApiClient.GetUserRolesAsync(string.Empty, Guid.NewGuid());

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);

        Logger.LogDebug(response.ProblemDetails.ToJson());
    }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the get user roles endpoint without an invalid access token.
    /// </summary>
    [Fact]
    public async Task Get_UserRoles_Sandbox_Invalid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var httpClient = HttpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);
        var apiClient = new MoneyMoovApiClient(httpClient);
        var merchantApiClient = new MerchantClient(apiClient);

        var response = await merchantApiClient.GetUserRolesAsync("xxx", Guid.NewGuid());

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);

        Logger.LogDebug(response.ProblemDetails.ToJson());
    }

    /// <summary>
    /// Tests that the an unauthorized response is received when an attempt is made to call
    /// the get user roles endpoint with a merchant access token.
    /// </summary>
    [Fact]
    public async Task Get_UserRoles_Sandbox_Unauthorised_Merchant_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var httpClient = HttpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);
        var apiClient = new MoneyMoovApiClient(httpClient);
        var merchantApiClient = new MerchantClient(apiClient);

        var response = await merchantApiClient.GetUserRolesAsync(SandboxAccessToken, SandboxMerchantID);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);
    }
}
