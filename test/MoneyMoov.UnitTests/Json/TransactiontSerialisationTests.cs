//-----------------------------------------------------------------------------
// Filename: TransactionSerialisationTests.cs
//
// Description: Unit tests for serialising a Transaction model to JSON.
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

public class TransactionSerialisationTests : MoneyMoovUnitTestBase<TransactionSerialisationTests>
{
    public TransactionSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    [Fact]
    public void Serialize_Transaction_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var transaction = new Transaction
        {
            Balance = 42.42m,
            Currency = CurrencyTypeEnum.EUR,
            Amount = 11.99m
        };

        var json = transaction.ToJsonFormatted();

        Logger.LogDebug($"json: {json}");

        Assert.Contains("\"balance\": 42.42,", json);
        Assert.Contains("\"balanceMinorUnits\": 4242,", json);
        Assert.Contains("\"amount\": 11.99,", json);
        Assert.Contains("\"amountMinorUnits\": 1199,", json);
    }
}