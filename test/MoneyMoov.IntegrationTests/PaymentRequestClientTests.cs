//-----------------------------------------------------------------------------
// Filename: PaymentRequestClientTests.cs
//
// Description: Integrations tests for the MoneyMoov PaymetnRequest API client.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
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
public class PaymentRequestClientTests : MoneyMoovTestBase<PaymentRequestClientTests>
{
    public PaymentRequestClientTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a payment request can be created and retrieved on the sandbox cluster.
    /// </summary>
    [Fact]
    public async Task Create_And_Get_PaymentRequest_Sandbox_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var httpClient = HttpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(MoneyMoovUrlBuilder.SANDBOX_MONEYMOOV_BASE_URL);
        var apiClient = new MoneyMoovApiClient(httpClient);
        var paymentRequestApiClient = new PaymentRequestClient(apiClient);

        var paymentRequestCreate = new PaymentRequestCreate
        {
            Description = "Create_Payment_Request_Sandbox_Test_Delete_Me",
            Amount = 2.0M,
            Currency = CurrencyTypeEnum.EUR,
            BaseOriginUrl = "https://localhost"
        };
        var createResponse = await paymentRequestApiClient.CreatePaymentRequestAsync(SandboxMerchantAccessToken, paymentRequestCreate);

        Assert.NotNull(createResponse);
        Assert.Equal(System.Net.HttpStatusCode.Created, createResponse.StatusCode);
        Assert.True(createResponse.Data.IsSome);
        Assert.True(createResponse.ProblemDetails.IsEmpty);

        var paymentRequest = (PaymentRequest)createResponse.Data;
        Logger.LogDebug(System.Text.Json.JsonSerializer.Serialize(paymentRequest));

        // Retrieve
        var getResponse = await paymentRequestApiClient.GetPaymentRequestAsync(SandboxMerchantAccessToken, paymentRequest.ID);

        // TODO: This fails with a UserAccessToken when it should succeed.

        Assert.NotNull(getResponse);
        Assert.Equal(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
        Assert.True(getResponse.Data.IsSome);
        Assert.True(getResponse.ProblemDetails.IsEmpty);

        var paymentRequest2 = (PaymentRequest)createResponse.Data;
        Logger.LogDebug(System.Text.Json.JsonSerializer.Serialize(paymentRequest2));
    }
}