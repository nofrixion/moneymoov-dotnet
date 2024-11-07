//-----------------------------------------------------------------------------
// Filename: PayrunInvoiceValidatorTests.cs
//
// Description: Unit tests for PayrunInvoice validation logic.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Jul 2024  Aaron Clauson   Created, Carne, Wexford, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PayrunInvoiceValidatorTests
{
    readonly ILogger<PayrunInvoiceValidatorTests> _logger;
    private LoggerFactory _loggerFactory;

    public PayrunInvoiceValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PayrunInvoiceValidatorTests>();
    }

    /// <summary>
    /// Tests that an invoice destination account Name is validated successfully. Note
    /// because the payment procescor is generalyl not known when an invoice is being validated
    /// the validation rules for all processor are applied.
    /// </summary>
    [Theory]
    [InlineData("A")]
    [InlineData("1-A")]
    [InlineData("1-A-2-c")]
    [InlineData("NFXN...LTD")]
    public void Validate_Account_Name_Success(string accountName)
    {
        var payrunInvoice = new PayrunInvoice
        { 
            InvoiceReference = "ref-1", 
            DestinationAccountName = accountName, 
            Currency = CurrencyTypeEnum.EUR, 
            TotalAmount = 11.00M,
            DestinationIban = "IE83MOCK91012396989925"
        };

        var problem = payrunInvoice.Validate();

        _logger.LogDebug(problem.ToJsonFormatted());

        Assert.True(problem.IsEmpty);
    }

    /// <summary>
    /// Tests that an invoice destination account Name is fails validation. Note
    /// because the payment procescor is generalyl not known when an invoice is being validated
    /// the validation rules for all processor are applied.
    /// </summary>
    [Theory]
    [InlineData("/")] // No letter or number.
    [InlineData("--")]// No letter or number.
    [InlineData(".-/&")] // No letter or number.
    [InlineData("1-A-2-c + + dfg")] // Invalid character '+'.
    [InlineData("Big Bucks £")]// Invalid character '£'.
    [InlineData("Big Bucks €")]// Invalid character '€'.
    [InlineData(":")] // Invalid initial character, can't be ':' or '-'.
    [InlineData("1-A-2-c + ! + dfg")] // Invalid character '!'.
    [InlineData(".-/& a")] // BC don't support '&' in account name.
    [InlineData("/:")] // BC don't support '/' in account name.
    [InlineData("TELECOMUNICAÇÕES S.A.")] // BC don't support unicode.
    [InlineData("Ç_é")] // BC don't support unicode.
    [InlineData("NFXN: LTD")] // Modulr don't support ':'
    public void Validate_Account_Name_Fails(string accountName)
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = accountName,
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            DestinationIban = "IE83MOCK91012396989925"
        };

        var problem = payrunInvoice.Validate();

        _logger.LogDebug(problem.Detail);

        Assert.False(problem.IsEmpty);
    }

    /// <summary>
    /// Validates that an invoice with a valid IBAN passes validation.
    /// </summary>
    [Fact]
    public void Validate_Payout_Valid_IBAN_Success()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = "Some Biz",
            DestinationIban = "IE83MOCK91012396989925",
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        Assert.True(result.IsEmpty);
    }

    /// <summary>
    /// Validates that an invoice with an invalid IBAN fails validation.
    /// </summary>
    [Fact]
    public void Validate_Payout_Invalid_IBAN_Fails()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = "Some Biz",
            DestinationIban = "IE36ULSB98501017331006",
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        _logger.LogDebug(result.ToTextErrorMessage());

        Assert.False(result.IsEmpty);
        Assert.Single(result.Errors);
    }

    /// <summary>
    /// Validates that an invoice with a valid SCAN details passes validation.
    /// </summary>
    [Fact]
    public void Validate_Payout_Valid_SCAN_Success()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = "Some Biz",
            DestinationSortCode = "123456",
            DestinationAccountNumber = "12345678",
            Currency = CurrencyTypeEnum.GBP,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        Assert.True(result.IsEmpty);
    }

    /// <summary>
    /// Tests than a EUR invoice mssing an IBAN fails validation.
    /// </summary>
    [Fact]
    public void Invoice_Missing_IBAN_Failure()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = "Some Biz",
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();
        
        _logger.LogDebug(result.ToJsonFormatted());
        
        Assert.False(result.IsEmpty);
    }

    /// <summary>
    /// Tests than a GBP invoice mssing SCAN details fails validation.
    /// </summary>
    [Fact]
    public void Invoice_Missing_SCAN_Failure()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            DestinationAccountName = "Some Biz",
            Currency = CurrencyTypeEnum.GBP,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        _logger.LogDebug(result.ToJsonFormatted());

        Assert.False(result.IsEmpty);
    }
}
