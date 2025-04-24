//-----------------------------------------------------------------------------
// Filename: PaymentAccountSerialisationTests.cs
//
// Description: Unit tests for serialising a PaymentAccount model to JSON.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 24 Apr 2025  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License:  
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests.Json;

public class PaymentAccountSerialisationTests : MoneyMoovUnitTestBase<PaymentAccountSerialisationTests>
{
    public PaymentAccountSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public void Serialize_PaymentAccount_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var paymentAccount = new PaymentAccount
        {
            Balance = 42.42m,
            Currency = CurrencyTypeEnum.EUR,
            SubmittedPayoutsBalance = 11.99m
        };

        var json = paymentAccount.ToJsonFormatted();

        Logger.LogDebug($"json: {json}");

        Assert.Contains("\"balance\": 42.42,", json);
        Assert.Contains("\"balanceMinorUnits\": 4242,", json);
        Assert.Contains("\"submittedPayoutsBalance\": 11.99,", json);
        Assert.Contains("\"submittedPayoutsBalanceMinorUnits\": 1199,", json);
        Assert.Contains("\"availableBalance\": 30.43,", json);
        Assert.Contains("\"availableBalanceMinorUnits\": 3043,", json);
    }
}