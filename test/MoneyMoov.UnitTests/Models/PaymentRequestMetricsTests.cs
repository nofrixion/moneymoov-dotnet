//-----------------------------------------------------------------------------
// Filename: PaymentRequestMetricsTests.cs
//
// Description: .
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Jul 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PaymentRequestMetricsTests : MoneyMoovUnitTestBase<PaymentRequestMetricsTests>
{
    public PaymentRequestMetricsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a payment request metrics instance can be correctly deserialised from Json.
    /// </summary>
    [Fact]
    public void Json_Deserialisation_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        string metricsJson = " {\"all\":10,\"unpaid\":1,\"partiallyPaid\":3,\"paid\":2,\"authorized\":4,\"totalAmountsByCurrency\":{\"all\":{\"eur\":329.01000000000},\"paid\":{\"eur\":103.00000000000},\"unpaid\":{\"eur\":100.00000000000},\"partiallyPaid\":{\"eur\":101.00000000000},\"authorized\":{\"eur\":0.01000000000}}}";

        var metrics = metricsJson.FromJson<PaymentRequestMetrics>();

        Assert.NotNull(metrics);

        Assert.Equal(10, metrics?.All);
        Assert.Equal(2, metrics?.Paid);
        Assert.Equal(3, metrics?.PartiallyPaid);
        Assert.Equal(1, metrics?.Unpaid);
        Assert.Equal(4, metrics?.Authorized);

        Assert.Contains(metrics?.TotalAmountsByCurrency[MetricsEnum.All.ToString().ToLower()]!, x => x.Key == CurrencyTypeEnum.EUR.ToString().ToLower());
        Assert.Equal(329.01m, metrics?.TotalAmountsByCurrency[MetricsEnum.All.ToString().ToLower()][CurrencyTypeEnum.EUR.ToString().ToLower()]);
    }
}