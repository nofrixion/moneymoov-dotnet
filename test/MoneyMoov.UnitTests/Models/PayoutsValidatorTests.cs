//-----------------------------------------------------------------------------
// Filename: PayoutsValidatorTests.cs
//
// Description: Unit tests for Payouts validation logic.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
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

public class PayoutsValidatorTests
{
    readonly ILogger<PayoutsValidatorTests> _logger;
    private LoggerFactory _loggerFactory;

    public PayoutsValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PayoutsValidatorTests>();
    }

    /// <summary>
    /// Tests that a payout property TheirReference is validated successfully.
    /// </summary>
    [Theory]
    [InlineData("refe-12")]
    [InlineData("&./refe-12")]
    [InlineData("r12 hsd-2")]
    [InlineData("-sd73/sdf.48&")]
    [InlineData("-sD7 K sdf -")]
    public void PaymentsValidator_ValidateTheirReference_Success(string theirReference)
    {
        var result = PayoutsValidator.ValidateTheirReference(theirReference, MoneyMoov.AccountIdentifierType.IBAN);

        Assert.True(result);
    }

    /// <summary>
    /// Tests that an invalid payout TheirReference fails validation.
    /// </summary>
    [Theory]
    [InlineData("re.e-1-", AccountIdentifierType.IBAN)]             //lower limit 6 alphanumeric (. and - not counted) 
    [InlineData("-sd6tu89-73.sdf./48 2983", AccountIdentifierType.SCAN)] //char upper limit 18
    [InlineData("-sD7!&K.sdf./", AccountIdentifierType.IBAN)]       //Invalid character '!'.
    [InlineData("ddddddddd", AccountIdentifierType.IBAN)]           //all same char
    [InlineData("Saldo F16 + F20", AccountIdentifierType.IBAN)]     // Invalid character '+'.
    [InlineData("Saldo F16 _ F20", AccountIdentifierType.IBAN)]     // Invalid character '_'.
    public void PaymentsValidator_ValidateTheirReference_Fail(string theirReference, AccountIdentifierType identifierType)
    {
        var result = PayoutsValidator.ValidateTheirReference(theirReference, identifierType);

        Assert.False(result);
    }

    /// <summary>
    /// Tests that a payout Account Name is validated successfully.
    /// </summary>
    [Theory]
    [InlineData("A")]
    [InlineData("1-A")]
    [InlineData("1-A-2-c")]
    [InlineData(".-/& a")]
    [InlineData("TELECOMUNICAÇÕES S.A.")]
    [InlineData("Ç_é")]
    public void PaymentsValidator_Validate_Account_Name_Success(string accountName)
    {
        var result = PayoutsValidator.IsValidAccountName(accountName);

        Assert.True(result);
    }

    /// <summary>
    /// Tests that an invalid payout Account Name fails validation.
    /// </summary>
    [Theory]
    [InlineData("/")] // No letter or number.
    [InlineData("--")]// No letter or number.
    [InlineData(".-/&")] // No letter or number.
    [InlineData("1-A-2-c + + dfg")] // Invalid character '+'.
    [InlineData("Big Bucks £")]// Invalid character '£'.
    [InlineData("Big Bucks €")]// Invalid character '€'.
    public void PaymentsValidator_Validate_Account_Name_Fails(string accountName)
    {
        var result = PayoutsValidator.IsValidAccountName(accountName);

        Assert.False(result);
    }

    /// <summary>
    /// Tests that a payout property YourReference is validated successfully.
    /// </summary>
    [Theory]
    [InlineData("")]
    [InlineData("refe-12")]
    [InlineData("r12 hsd-2")]
    [InlineData("safe1234 wf fwef ew e ergerger g")]
    public void PaymentsValidator_ValidateYourReference_Success(string yourReference)
    {
        var result = PayoutsValidator.ValidateYourReference(yourReference);

        Assert.True(result);
    }

    /// <summary>
    /// Tests that an invalid payout YourReference fails validation.
    /// </summary>
    [Theory]
    [InlineData("re.e-1-")]             // Invalid char '.'. 
    [InlineData("-sD7!&K.sdf./")]       //Invalid character '!'.
    [InlineData("Saldo F16 + F20")]     // Invalid character '+'.
    public void PaymentsValidator_ValidateYourReference_Fail(string yourReference)
    {
        var result = PayoutsValidator.ValidateYourReference(yourReference);

        Assert.False(result);
    }

    /// <summary>
    /// Validates that a payout with an invalid IBAN returns the corect result.
    /// </summary>
    [Fact]
    public void Validate_Payout_Invalid_IBAN_Fails()
    {
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            AccountID = Guid.Parse("B2DBB4E1-5F8A-4B07-82A0-EB033E6F3421"),
            Type = AccountIdentifierType.IBAN,
            Description = "Xero Invoice fgfg from Demo Company (Global).",
            Currency = CurrencyTypeEnum.EUR,
            Amount = 11.00M,
            YourReference = "xero-18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            TheirReference = "Placeholder",
            Status = PayoutStatus.PENDING_INPUT,
            InvoiceID = "18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            Destination = new Counterparty
            {
                Name = "Joe Bloggs",
                Identifier = new AccountIdentifier
                {
                    IBAN = "IE36ULSB98501017331006",
                    Currency = CurrencyTypeEnum.EUR
                }
            }
        };

        var result = payout.Validate();

        _logger.LogDebug(result.ToTextErrorMessage());

        Assert.False(result.IsEmpty);
        Assert.Single(result.Errors);
    }

    public enum ComparisonMethod
    {
        Equals,
        StartsWith
    }

    /// <summary>
    /// Tests that making a safe TheirReference works as expected.
    /// </summary>
    [Theory]
    [InlineData(AccountIdentifierType.IBAN, "", "NFXN", ComparisonMethod.StartsWith)]
    [InlineData(AccountIdentifierType.IBAN, "xyz123", "xyz123", ComparisonMethod.Equals)]
    [InlineData(AccountIdentifierType.IBAN, "-sD7&K.sdf./", "-sD7&K.sdf./", ComparisonMethod.Equals)] // If valid, gets left alone.
    [InlineData(AccountIdentifierType.IBAN, "-sD7!&K.sdf./", "sD7Ksdf", ComparisonMethod.Equals)] // If invalid, non-ASCII chars get replaced.
    [InlineData(AccountIdentifierType.IBAN, "Acme™ Corporation", "Acme Corporation", ComparisonMethod.Equals)] // Replace unicode.
    [InlineData(AccountIdentifierType.SCAN, "Too long for SCAN which oly likes short refs", "Too long for SCAN", ComparisonMethod.Equals)]
    public void PaymentsValidator_Make_Safe_TheirReference(AccountIdentifierType accountType, 
        string requestedTheirReference, 
        string expectedTheirReference,
        ComparisonMethod comparison)
    {
        var safeTheirRef = PayoutsValidator.MakeSafeTheirReference(accountType, requestedTheirReference);

        _logger.LogDebug($"Safe their ref={safeTheirRef}.");

        Assert.True(PayoutsValidator.ValidateTheirReference(safeTheirRef, accountType));

        if (comparison == ComparisonMethod.Equals)
        {
            Assert.Equal(expectedTheirReference, safeTheirRef);
        }
        else if(comparison == ComparisonMethod.StartsWith)
        {
            Assert.StartsWith(expectedTheirReference, safeTheirRef);
        }
    }

    [Fact]
    public void GetReferencesFromInvoices()
    {
        // var more = " +3 more";
        
        var invoiceReferences = new List<string?>
        {
            "ref-17555555777777",
            "ref-1",
            "ref-2",
            "ref-3",
            "ref-4",
            "ref-5",
            "ref-6",
            "ref-7",
            "ref-8",
            "ref-9",
            "ref-10",
            "ref-11",
            "ref-12",
            "ref-13",
            "ref-14",
            "ref-15",
            "ref-16",
            "ref-175555557",
            "ref-18",
            "ref-19",
            "ref-20",
            "ref-21",
            "ref-22",
            "ref-23",
            "ref-24",
        };
        
        var theirReferenceIban = PayoutsValidator.GetTheirReferenceFromInvoices(CurrencyTypeEnum.EUR, invoiceReferences);
        
        _logger.LogDebug(theirReferenceIban);
        _logger.LogDebug("Their IBAN reference length={Length}", theirReferenceIban.Length);
        
        var theirReferenceScan = PayoutsValidator.GetTheirReferenceFromInvoices(CurrencyTypeEnum.GBP, invoiceReferences);
        
        _logger.LogDebug(theirReferenceScan);
        _logger.LogDebug("Their SCAN reference length={Length}", theirReferenceScan.Length);
        
        var yourReference = PayoutsValidator.GetYourReferenceFromInvoices(invoiceReferences);
        
        _logger.LogDebug(yourReference);
        _logger.LogDebug("Your reference length={YourReferenceLength}", yourReference.Length);
    }

    [Fact]
    public void PayoutsValidator_Validate_EUR_Destination_Identifier_Success()
    {
        var destination = new Counterparty
        {
            Name = "Joe Bloggs",
            Identifier = new AccountIdentifier
            {
                IBAN = "GB42MOCK00000070629907",
                Currency = CurrencyTypeEnum.EUR
            }
        };
        
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            AccountID = Guid.Parse("B2DBB4E1-5F8A-4B07-82A0-EB033E6F3421"),
            Type = AccountIdentifierType.IBAN,
            Description = "Xero Invoice fgfg from Demo Company (Global).",
            Currency = CurrencyTypeEnum.EUR,
            Amount = 11.00M,
            YourReference = "xero-18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            TheirReference = "Placeholder",
            Status = PayoutStatus.PENDING_INPUT,
            InvoiceID = "18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            Destination = destination
        };
        
        var result = payout.Validate();
        
        _logger.LogDebug(result.ToTextErrorMessage());
        
        Assert.True(result.IsEmpty);
    }
    
    [Fact]
    public void PayoutsValidator_Validate_GBP_Destination_Identifier_Success()
    {
        var destination = new Counterparty
        {
            Name = "Joe Bloggs",
            Identifier = new AccountIdentifier
            {
                SortCode = "123456",
                AccountNumber = "00000070629907",
                Currency = CurrencyTypeEnum.GBP
            }
        };
        
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            AccountID = Guid.Parse("B2DBB4E1-5F8A-4B07-82A0-EB033E6F3421"),
            Type = AccountIdentifierType.SCAN,
            Description = "Xero Invoice fgfg from Demo Company (Global).",
            Currency = CurrencyTypeEnum.GBP,
            Amount = 11.00M,
            YourReference = "xero-18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            TheirReference = "Placeholder",
            Status = PayoutStatus.PENDING_INPUT,
            InvoiceID = "18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            Destination = destination
        };
        
        var result = payout.Validate();
        
        _logger.LogDebug(result.ToTextErrorMessage());
        
        Assert.True(result.IsEmpty);
    }
    
    [Fact]
    public void PayoutsValidator_Validate_EUR_Destination_Identifier_Failure()
    {
        var destination = new Counterparty
        {
            Name = "Joe Bloggs",
            Identifier = new AccountIdentifier
            {
                Currency = CurrencyTypeEnum.EUR
            }
        };
        
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            AccountID = Guid.Parse("B2DBB4E1-5F8A-4B07-82A0-EB033E6F3421"),
            Type = AccountIdentifierType.IBAN,
            Description = "Xero Invoice fgfg from Demo Company (Global).",
            Currency = CurrencyTypeEnum.EUR,
            Amount = 11.00M,
            YourReference = "xero-18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            TheirReference = "Placeholder",
            Status = PayoutStatus.PENDING_INPUT,
            InvoiceID = "18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            Destination = destination
        };
        
        var result = payout.Validate();
        
        _logger.LogDebug(result.ToTextErrorMessage());
        
        Assert.False(result.IsEmpty);
    }
    
    [Fact]
    public void PayoutsValidator_Validate_GBP_Destination_Identifier_Failure()
    {
        var destination = new Counterparty
        {
            Name = "Joe Bloggs",
            Identifier = new AccountIdentifier
            {
                Currency = CurrencyTypeEnum.GBP
            }
        };
        
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            AccountID = Guid.Parse("B2DBB4E1-5F8A-4B07-82A0-EB033E6F3421"),
            Type = AccountIdentifierType.SCAN,
            Description = "Xero Invoice fgfg from Demo Company (Global).",
            Currency = CurrencyTypeEnum.GBP,
            Amount = 11.00M,
            YourReference = "xero-18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            TheirReference = "Placeholder",
            Status = PayoutStatus.PENDING_INPUT,
            InvoiceID = "18ead957-e3bc-4b12-b5c6-d12e4bef9d24",
            Destination = destination
        };
        
        var result = payout.Validate();
        
        _logger.LogDebug(result.ToTextErrorMessage());
        
        Assert.False(result.IsEmpty);
    }
    
}
