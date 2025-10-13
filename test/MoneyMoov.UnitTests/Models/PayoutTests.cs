//-----------------------------------------------------------------------------
// Filename: PayoutTests.cs
//
// Description: Unit tests for Payouts model.
//
// Author(s):
// Aaron Clauson (arif@nofrixion.com)
// 
// History:
// 12 Jun 2022  Arif Matin  Created, Harcourt Street, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PayoutTests
{
    readonly ILogger _logger;
    private LoggerFactory _loggerFactory;

    public PayoutTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PayoutTests>();
    }

    [Fact]
    public void FxFormattedDestinationAmount_ReturnsEmpty_WhenCurrencyOrRateIsNull()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = null,
            FxRate = 1.1m,
            FxDestinationAmount = 100m,
            Amount = 200m
        };

        Assert.Equal("?100.00", payout.FormattedFxDestinationAmount);

        payout.FxDestinationCurrency = CurrencyTypeEnum.EUR;
        payout.FxRate = null;

        Assert.Equal("€100.00", payout.FormattedFxDestinationAmount);
    }

    [Fact]
    public void FxFormattedDestinationAmount_ReturnsEmpty_WhenAllInputsAreNull()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = null,
            FxRate = null,
            FxDestinationAmount = null,
            Amount = 100m
        };

        Assert.Equal("?0.00", payout.FormattedFxDestinationAmount);
    }

    [Fact]
    public void FxFormattedDestinationAmount_ReturnsEmpty_WhenOnlyCurrencyIsNull()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = null,
            FxRate = 1.2m,
            FxDestinationAmount = 100m,
            Amount = 200m
        };

        Assert.Equal("?100.00", payout.FormattedFxDestinationAmount);
    }

    [Fact]
    public void FxFormattedDestinationAmount_ReturnsEmpty_WhenOnlyRateIsNull_AndDestAmountMissing()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = CurrencyTypeEnum.USD,
            FxRate = null,
            FxDestinationAmount = null,
            Amount = 300m
        };

        Assert.Equal("$0.00", payout.FormattedFxDestinationAmount);
    }

    [Fact]
    public void FxFormattedDestinationAmount_UsesDestinationAmount_WhenAllInputsAreValid()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = CurrencyTypeEnum.EUR,
            FxRate = 0.8m,
            FxDestinationAmount = 123.45m,
            Amount = 100m
        };

        var expected = PaymentAmount.DisplayCurrencyAndAmount(CurrencyTypeEnum.EUR, 123.45m);
        Assert.Equal(expected, payout.FormattedFxDestinationAmount);
    }

    [Fact]
    public void FxFormattedDestinationAmount_Computes_WhenDestinationAmountIsNull_ButOtherValuesSet()
    {
        var payout = new Payout
        {
            FxDestinationCurrency = CurrencyTypeEnum.GBP,
            FxRate = 1.1m,
            FxDestinationAmount = 82.5m,
            Amount = 75m
        };

        var expected = PaymentAmount.DisplayCurrencyAndAmount(CurrencyTypeEnum.GBP, 82.5m);
        Assert.Equal(expected, payout.FormattedFxDestinationAmount);
    }
}
