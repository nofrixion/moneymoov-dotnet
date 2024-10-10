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
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class PayoutSerialisationTests : MoneyMoovUnitTestBase<PayoutSerialisationTests>
{
    public PayoutSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public async Task Serialize_Payout_Success()
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
}