//-----------------------------------------------------------------------------
// Filename: PayoutUpdateSerialisationTests.cs
//
// Description: Unit tests for serialising a PayoutUpdate model to JSON.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 21 Mar 2025  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class PayoutUpdateSerialisationTests : MoneyMoovUnitTestBase<PayoutSerialisationTests>
{
    public PayoutUpdateSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public void Deserialise_PayoutUpdate_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var payoutUpdateJson = @"
{
  ""AccountID"": ""17b135e2-a5ef-4124-97c3-1776b2608504"",
  ""Currency"": ""USD"",
  ""Amount"": ""123"",
  ""Description"": ""werwer"",
  ""YourReference"": """",
  ""TheirReference"": ""werwer"",
  ""Type"": """",
  ""Scheduled"": false,
  ""ScheduleDate"": null,
  ""PaymentRail"": ""Default"",
  ""Destination"": {
    ""accountID"": ""bfea278b-af5e-4208-89e3-fba16a028711""
  }
}
";

        var payoutUpdate = payoutUpdateJson.FromJson<PayoutUpdate>();

        Assert.NotNull(payoutUpdate);
        Assert.NotNull(payoutUpdate.Destination);
        Assert.Null(payoutUpdate.Destination.Identifier);
    }
}