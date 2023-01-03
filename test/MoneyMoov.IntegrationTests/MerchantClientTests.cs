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
using NoFrixion.MoneyMoov.Models;
using Xunit.Abstractions;

namespace MoneyMoov.IntegrationTests;

[Trait("Category", "integration")]
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

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

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

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

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

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetUserRolesAsync(SandboxMerchantAccessToken, SandboxMerchantID);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);
    }

    /// <summary>
    /// Tests that the a success response is received when calling the get user roles endpoint 
    /// with a user access token.
    /// </summary>
    [Fact]
    public async Task Get_UserRoles_Sandbox_User_Token_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetUserRolesAsync(SandboxUserAccessToken, SandboxMerchantID);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.False(response.Data.IsNone);
        Assert.True(response.ProblemDetails.IsEmpty);

        var userRoles = (List<UserRole>)response.Data;
        Assert.NotEmpty(userRoles);
    }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the get merchant tokens endpoint without an access token.
    /// </summary>
    [Fact]
    public async Task Get_MerchantTokens_Sandbox_No_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetMerchantTokensAsync(string.Empty, Guid.NewGuid());

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.PreconditionFailed, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);

        Logger.LogDebug(response.ProblemDetails.ToJson());
    }

    /// <summary>
    /// Tests that the expected error response is received when an attempt is made to call
    /// the get merchant tokens endpoint without an invalid access token.
    /// </summary>
    [Fact]
    public async Task Get_MerchantTokens_Sandbox_Invalid_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetMerchantTokensAsync("xxx", Guid.NewGuid());

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);

        Logger.LogDebug(response.ProblemDetails.ToJson());
    }

    /// <summary>
    /// Tests that the an unauthorized response is received when an attempt is made to call
    /// the get merchant tokens endpoint with a merchant access token.
    /// </summary>
    [Fact]
    public async Task Get_MerchantTokens_Sandbox_Unauthorised_Merchant_Token_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetMerchantTokensAsync(SandboxMerchantAccessToken, SandboxMerchantID);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(response.Data.IsNone);
        Assert.False(response.ProblemDetails.IsEmpty);
    }

    /// <summary>
    /// Tests that the a success response is received when  the get merchant tokens 
    /// endpoint with a user access token.
    /// </summary>
    [Fact]
    public async Task Get_MerchantTokens_Sandbox_User_Token_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var response = await merchantApiClient.GetMerchantTokensAsync(SandboxUserAccessToken, SandboxMerchantID);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.False(response.Data.IsNone);
        Assert.True(response.ProblemDetails.IsEmpty);
    }

    /// <summary>
    /// Tests that the create merchant token method can be correctly called on the sandbox cluster.
    /// </summary>
    [Fact]
    public async Task Create_And_Delete_Merchant_Token_Sandbox_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var merchantApiClient = new MerchantClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);

        var tokenAdd = new TokenAdd
        {
            Description = "Create_Merchant_Token_Sandbox_Test_Delete_Me",
            MerchantID = SandboxMerchantID
        };
        var response = await merchantApiClient.CreateMerchantTokenAsync(SandboxUserAccessToken, tokenAdd);

        Assert.NotNull(response);
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.True(response.Data.IsSome);
        Assert.True(response.ProblemDetails.IsEmpty);

        var merchantToken = (MerchantToken)response.Data;
        Logger.LogDebug(System.Text.Json.JsonSerializer.Serialize(merchantToken));

        // Clean up.
        var tokensApiClient = new TokensClient(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);
        var deleteResponse = await tokensApiClient.DeleteTokenAsync(SandboxUserAccessToken, merchantToken.ID);

        Assert.NotNull(deleteResponse);
        Assert.Equal(System.Net.HttpStatusCode.OK, deleteResponse.StatusCode);
        Assert.True(deleteResponse.ProblemDetails.IsEmpty);
    }
}
