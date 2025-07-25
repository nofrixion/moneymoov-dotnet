//-----------------------------------------------------------------------------
// Filename: PayoutSerialisationTests.cs
//
// Description: Unit tests for serialising a Payout model to JSON.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 10 Oct 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using System.Net.Http.Json;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class PayoutSerialisationTests : MoneyMoovUnitTestBase<PayoutSerialisationTests>
{
    public PayoutSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public void Serialize_Payout_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var payout = new Payout
        {
            Amount = 42.42m,
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "123 456",
            Status = PayoutStatus.PROCESSED,
            SourceAccountCurrency = CurrencyTypeEnum.EUR,
            SourceAccountAvailableBalance = 13.13m,
        };

        var json = payout.ToJsonFormatted();

        Logger.LogDebug($"json: {json}");

        Assert.Contains("\"amount\": 42.42,", json);
        Assert.Contains("\"amountMinorUnits\": 4242,", json);
        Assert.Contains("\"sourceAccountAvailableBalance\": 13.13,", json);
        Assert.Contains("\"sourceAccountAvailableBalanceMinorUnits\": 1313,", json);
    }

    [Fact]
    public async Task Serialize_Payout_Content_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var payout = new Payout
        {
            Amount = 42.42m,
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "123 456",
            Status = PayoutStatus.PROCESSED
        };

        var json = payout.ToJsonFlat();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        Logger.LogDebug($"json: {json}");
        Logger.LogDebug($"content: {await content.ReadAsStringAsync()}");
    }

    /// <summary>
    /// Tests a payout serialisation with System.Text, which is what's used for webhook notifications.
    /// </summary>
    [Fact]
    public async Task Serialize_Payout_SystemText_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var payout = new Payout
        {
            Amount = 42m,
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "123 456",
            Status = PayoutStatus.PROCESSED
        };

        var content = JsonContent.Create(payout, options: MoneyMoovJson.GetSystemTextSerialiserOptions());

        var json = await content.ReadAsStringAsync();

        Logger.LogDebug($"json: {json}");

        Assert.Contains("\"amount\":42.00", json); // Check that the amount is serialised with two decimal places.
    }

    /// <summary>
    /// Tests a payout serialisation with Newtonsoft, which is what's used for API responses.
    /// </summary>
    [Fact]
    public void Serialize_Payout_Newtonsoft_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var payout = new Payout
        {
            Amount = 42m,
            Currency = CurrencyTypeEnum.EUR,
            TheirReference = "123 456",
            Status = PayoutStatus.PROCESSED
        };

        var json = payout.ToJsonFlat();

        Logger.LogDebug($"json: {json}");

        Assert.Contains("\"amount\":42.00", json); // Check that the amount is serialised with two decimal places.
    }
}