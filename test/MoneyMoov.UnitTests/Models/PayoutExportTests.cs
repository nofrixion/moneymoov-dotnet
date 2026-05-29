//  -----------------------------------------------------------------------------
//   Filename: PayoutExportTests.cs
// 
//   Description: Unit tests for Payout CSV export.
// 
//   Author(s):
//   Constantine Nalimov
//   Axel Granillo (axel@nofrixion.com)
// 
//   History:
//   05 Mar 2026  Constantine Nalimov  Created.
//   05 Jul 2026  Axel Granillo        Added FX column tests.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

using System.Globalization;
using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;

namespace MoneyMoov.UnitTests.Models;

public class PayoutExportTests
{
    [Fact]
    public async Task Payout_ToCsvRow_MatchesSnapshot()
    {
        var payout = new Payout
        {
            ID = Guid.Parse("12345678-1234-1234-1234-123456789012"),
            AccountID = Guid.Parse("22345678-1234-1234-1234-123456789012"),
            MerchantID = Guid.Parse("32345678-1234-1234-1234-123456789012"),
            Amount = 100.50m,
            Currency = CurrencyTypeEnum.EUR,
            Type = AccountIdentifierType.BIC,
            Description = "Test Payout",
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier
                {
                    BIC = "TESTBIC1",
                    IBAN = "TESTIBAN1",
                    Currency = CurrencyTypeEnum.EUR
                },
                Name = "John Doe"
            },
            Status = PayoutStatus.QUEUED,
            Inserted = new DateTimeOffset(2025, 3, 5, 12, 0, 0, TimeSpan.Zero),
            LastUpdated = new DateTimeOffset(2025, 3, 5, 12, 30, 0, TimeSpan.Zero)
        };

        var csvRow = payout.ToCsvRow();
        
        await Verify(csvRow);
    }

    [Fact]
    public void Payout_CsvHeader_ColumnCount_Matches_ToCsvRow()
    {
        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier
                {
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        var header = PayoutExtensions.GetCsvHeader();
        var row = payout.ToCsvRow();

        var headerColumnCount = header.Split(',').Length;
        var rowColumnCount = row.Split(',').Length;

        Assert.Equal(headerColumnCount, rowColumnCount);
    }

    /// <summary>
    /// Verifies the CSV header ends with the six FX column names in the correct order.
    /// </summary>
    [Fact]
    public void Payout_CsvHeader_EndsWithFxColumns()
    {
        var header = PayoutExtensions.GetCsvHeader();
        var columns = header.Split(',');

        var lastSix = columns[^6..];

        Assert.Equal("FxDestinationCurrency", lastSix[0]);
        Assert.Equal("IndicativeFxRate", lastSix[1]);
        Assert.Equal("IndicativeFxDestinationAmount", lastSix[2]);
        Assert.Equal("TransactedFxRate", lastSix[3]);
        Assert.Equal("TransactedFxDestinationAmount", lastSix[4]);
        Assert.Equal("TransactedFxSourceAmount", lastSix[5]);
    }

    /// <summary>
    /// Verifies that non-null indicative and transacted FX values are correctly formatted
    /// in the CSV row using invariant culture decimals and enum ToString for currency.
    /// </summary>
    [Theory]
    [InlineData(1.2345, 150.00, "EUR", 1.2340, 149.95, 123.45)]
    [InlineData(0.8567, 1000.5678, "GBP", 0.8560, 999.99, 1168.00)]
    [InlineData(1.0, 0.01, "USD", 1.00000001, 0.01, 0.01)]
    public void Payout_ToCsvRow_NonNullFxValues_FormatsCorrectly(
        double fxRate,
        double fxDestAmount,
        string fxDestCurrency,
        double transactedFxRate,
        double transactedFxAmount,
        double transactedAmount)
    {
        var fxRateDecimal = (decimal)fxRate;
        var fxDestAmountDecimal = (decimal)fxDestAmount;
        var transactedFxRateDecimal = (decimal)transactedFxRate;
        var transactedFxAmountDecimal = (decimal)transactedFxAmount;
        var transactedAmountDecimal = (decimal)transactedAmount;
        var currency = Enum.Parse<CurrencyTypeEnum>(fxDestCurrency);

        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier { Currency = CurrencyTypeEnum.EUR }
            },
            FxRate = fxRateDecimal,
            FxDestinationAmount = fxDestAmountDecimal,
            FxDestinationCurrency = currency,
            TransactedFxRate = transactedFxRateDecimal,
            TransactedFxAmount = transactedFxAmountDecimal,
            TransactedAmount = transactedAmountDecimal
        };

        var row = payout.ToCsvRow();
        var columns = row.Split(',');

        // The last six columns are the FX fields (quoted by ToSafeCsvString)
        var expectedFxRate = $"\"{fxRateDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        var expectedFxDestAmount = $"\"{fxDestAmountDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        var expectedFxDestCurrency = $"\"{currency}\"";
        var expectedTransactedFxRate = $"\"{transactedFxRateDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        var expectedTransactedFxAmount = $"\"{transactedFxAmountDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        var expectedTransactedAmount = $"\"{transactedAmountDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        
        Assert.Equal(expectedFxDestCurrency, columns[^6]);
        Assert.Equal(expectedFxRate, columns[^5]);
        Assert.Equal(expectedFxDestAmount, columns[^4]);
        Assert.Equal(expectedTransactedFxRate, columns[^3]);
        Assert.Equal(expectedTransactedFxAmount, columns[^2]);
        Assert.Equal(expectedTransactedAmount, columns[^1]);
    }

    /// <summary>
    /// Verifies that when all FX values are null, the CSV row outputs empty strings
    /// for all six FX columns.
    /// </summary>
    [Fact]
    public void Payout_ToCsvRow_AllFxValuesNull_OutputsEmptyStrings()
    {
        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier { Currency = CurrencyTypeEnum.EUR }
            },
            FxRate = null,
            FxDestinationAmount = null,
            FxDestinationCurrency = null,
            TransactedFxRate = null,
            TransactedFxAmount = null,
            TransactedAmount = null
        };

        var row = payout.ToCsvRow();
        var columns = row.Split(',');

        // Empty strings are not quoted by ToSafeCsvString
        Assert.Equal(string.Empty, columns[^6]);
        Assert.Equal(string.Empty, columns[^5]);
        Assert.Equal(string.Empty, columns[^4]);
        Assert.Equal(string.Empty, columns[^3]);
        Assert.Equal(string.Empty, columns[^2]);
        Assert.Equal(string.Empty, columns[^1]);
    }

    /// <summary>
    /// Verifies that when only transacted FX fields are null (single-currency payout scenario),
    /// the indicative fields are populated and transacted fields are empty strings.
    /// </summary>
    [Fact]
    public void Payout_ToCsvRow_OnlyTransactedFieldsNull_IndicativePopulated()
    {
        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier { Currency = CurrencyTypeEnum.EUR }
            },
            FxRate = 1.1234m,
            FxDestinationAmount = 500.00m,
            FxDestinationCurrency = CurrencyTypeEnum.GBP,
            TransactedFxRate = null,
            TransactedFxAmount = null,
            TransactedAmount = null
        };

        var row = payout.ToCsvRow();
        var columns = row.Split(',');

        // Indicative fields should be populated and quoted
        Assert.Equal("\"GBP\"", columns[^6]);
        Assert.Equal("\"1.1234\"", columns[^5]);
        Assert.Equal("\"500.00\"", columns[^4]);

        // Transacted fields should be empty
        Assert.Equal(string.Empty, columns[^3]);
        Assert.Equal(string.Empty, columns[^2]);
        Assert.Equal(string.Empty, columns[^1]);
    }

    /// <summary>
    /// Verifies that the number of columns in the CSV row always matches the header column count,
    /// regardless of which FX fields are null or non-null.
    /// </summary>
    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(false, false)]
    public void Payout_ToCsvRow_ColumnCount_AlwaysMatchesHeader(
        bool hasIndicativeFx,
        bool hasTransactedFx)
    {
        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier { Currency = CurrencyTypeEnum.EUR }
            },
            FxRate = hasIndicativeFx ? 1.5m : null,
            FxDestinationAmount = hasIndicativeFx ? 200.00m : null,
            FxDestinationCurrency = hasIndicativeFx ? CurrencyTypeEnum.USD : null,
            TransactedFxRate = hasTransactedFx ? 1.4999m : null,
            TransactedFxAmount = hasTransactedFx ? 199.50m : null,
            TransactedAmount = hasTransactedFx ? 133.00m : null
        };

        var header = PayoutExtensions.GetCsvHeader();
        var row = payout.ToCsvRow();

        var headerColumnCount = header.Split(',').Length;
        var rowColumnCount = row.Split(',').Length;

        Assert.Equal(headerColumnCount, rowColumnCount);
    }

    /// <summary>
    /// Verifies correct decimal formatting for various precisions and currency enum values
    /// in the FX columns.
    /// </summary>
    [Theory]
    [InlineData(0.0001, "EUR")]
    [InlineData(1234567.89, "GBP")]
    [InlineData(0, "USD")]
    public void Payout_ToCsvRow_FxDecimalPrecision_PreservedInOutput(
        double fxRateValue,
        string currencyName)
    {
        var fxRateDecimal = (decimal)fxRateValue;
        var currency = Enum.Parse<CurrencyTypeEnum>(currencyName);

        var payout = new Payout
        {
            Destination = new Counterparty
            {
                Identifier = new AccountIdentifier { Currency = CurrencyTypeEnum.EUR }
            },
            FxRate = fxRateDecimal,
            FxDestinationAmount = fxRateDecimal,
            FxDestinationCurrency = currency,
            TransactedFxRate = fxRateDecimal,
            TransactedFxAmount = fxRateDecimal,
            TransactedAmount = fxRateDecimal
        };

        var row = payout.ToCsvRow();
        var columns = row.Split(',');

        var expectedDecimal = $"\"{fxRateDecimal.ToString(CultureInfo.InvariantCulture)}\"";
        var expectedCurrency = $"\"{currency}\"";

        Assert.Equal(expectedCurrency, columns[^6]);
        Assert.Equal(expectedDecimal, columns[^5]);
        Assert.Equal(expectedDecimal, columns[^4]);
        Assert.Equal(expectedDecimal, columns[^3]);
        Assert.Equal(expectedDecimal, columns[^2]);
        Assert.Equal(expectedDecimal, columns[^1]);
    }
}
