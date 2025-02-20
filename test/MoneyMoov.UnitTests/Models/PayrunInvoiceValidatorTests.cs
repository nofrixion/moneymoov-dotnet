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
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            Destination = new Counterparty
            {
                Name = accountName,
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            }
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
    [InlineData("--")] // No letter or number.
    [InlineData(".-/&")] // No letter or number.
    [InlineData("1-A-2-c + + dfg")] // Invalid character '+'.
    [InlineData("Big Bucks £")] // Invalid character '£'.
    [InlineData("Big Bucks €")] // Invalid character '€'.
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
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            Destination = new Counterparty
            {
                Name = accountName,
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            }

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
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
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
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE36ULSB98501017331006",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
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
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    SortCode = "123456",
                    AccountNumber = "12345678",
                    Currency = CurrencyTypeEnum.GBP
                }
            },
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
            Destination =
                new Counterparty
                {
                    Name = "Some Biz",
                    Identifier = new AccountIdentifier
                    {
                        Currency = CurrencyTypeEnum.EUR
                    }
                },
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
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    Currency = CurrencyTypeEnum.GBP
                }
            },
            Currency = CurrencyTypeEnum.GBP,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        _logger.LogDebug(result.ToJsonFormatted());

        Assert.False(result.IsEmpty);
    }

    [Fact]
    public void Invoice_With_PaymentReference_MaxLength_Validates_Success()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            Destination = new Counterparty
            {
                Name = "Some Biz"
            },
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            PaymentReference = "12345678901234567890" // More than 18 characters.
        };

        var result = payrunInvoice.Validate();

        _logger.LogDebug(result.ToJsonFormatted());

        Assert.False(result.IsEmpty);
    }

    [Fact]
    public void Invoice_With_Empty_InvoiceReference_Returns_Error()
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "",
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        _logger.LogDebug(result.ToJsonFormatted());

        Assert.False(result.IsEmpty);
    }

    [Fact]
    public void Invoice_With_Obsolete_Reference_Field_Set_Success()
    {
        var payrunInvoice = new PayrunInvoice
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Reference = "ref-1",
#pragma warning restore CS0618 // Type or member is obsolete
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M
        };

        var result = payrunInvoice.Validate();

        Assert.True(result.IsEmpty);
    }
    
    [Theory]
    [InlineData("refe-12")]
    [InlineData("r12 hsd-2")]
    [InlineData("Saldo F16")]
    [InlineData("s-D7 K sdf -")]
    public void PayrunInvoice_PaymentReferenceValidation_Success(string theirReference)
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE83MOCK91012396989925",
                    Currency = CurrencyTypeEnum.EUR
                }
            },
            PaymentReference = theirReference
        };

        var result = payrunInvoice.Validate();

        Assert.True(result.IsEmpty);
    }
    
    [Theory]
    [InlineData("-sD7!&K.sdf./", AccountIdentifierType.IBAN)] // Invalid character '!'
    [InlineData("Saldo F16 + F20", AccountIdentifierType.IBAN)] // Invalid character '+'
    [InlineData("Saldo F16 _ F20", AccountIdentifierType.IBAN)] // Invalid character '_'
    [InlineData("dddddddd", AccountIdentifierType.IBAN)] // Only one distinct character
    [InlineData("-sd6tu89-73.sdf./48 2983", AccountIdentifierType.SCAN)] // Starts with '-'
    public void PayrunInvoice_PaymentReferenceValidation_Fail(string theirReference,
        AccountIdentifierType identifierType)
    {
        var payrunInvoice = new PayrunInvoice
        {
            InvoiceReference = "ref-1",
            Currency = CurrencyTypeEnum.EUR,
            TotalAmount = 11.00M,
            Destination = new Counterparty
            {
                Name = "Some Biz",
                Identifier = new AccountIdentifier
                {
                    IBAN = identifierType == AccountIdentifierType.IBAN ? "IE83MOCK91012396989925" : null,
                    AccountNumber = identifierType == AccountIdentifierType.SCAN ? "12345678" : null,
                    SortCode = identifierType == AccountIdentifierType.SCAN ? "123456" : null,
                    Currency = identifierType == AccountIdentifierType.IBAN
                        ? CurrencyTypeEnum.EUR
                        : CurrencyTypeEnum.GBP
                }
            },
            PaymentReference = theirReference
        };

        var result = payrunInvoice.Validate();

        Assert.False(result.IsEmpty);
    }
}
