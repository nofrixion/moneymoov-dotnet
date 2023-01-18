//-----------------------------------------------------------------------------
// Filename: PaymentsValidatorTests.cs
//
// Description: Unit tests for payment(payouts) validation logic.
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

public class PaymentsValidatorTests
{
    readonly ILogger<PaymentsValidatorTests> _logger;
    private LoggerFactory _loggerFactory;

    public PaymentsValidatorTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PaymentsValidatorTests>();
    }

    /// <summary>
    /// Checks the custom regex for the payout account name generates the same results as the 
    /// OpenAPI generated regex generated from the Modulr swagger file.
    /// </summary>
    //[Theory]
    //[InlineData("refe-12")]
    //public void Validate_Account_Name_Custom_OpenApi(string accountName)
    //{
    //    PaymentDestination payDest = new PaymentDestination(accountNumber: "123456", 
    //        name: accountName, 
    //        emailAddress: "some@some.com", 
    //        iban: "GB42MOCK00000070629907",
    //        sortCode: "000000");

    //    var context = new ValidationContext(payDest, serviceProvider: null, items: null);
    //    List<ValidationResult>? validationResults = new List<ValidationResult>();
    //    bool isValid = Validator.TryValidateObject(payDest, context, validationResults, true);

    //    Assert.False(isValid);

    //    foreach(var validationResult in validationResults)
    //    {
    //        _logger.LogDebug(validationResult.ErrorMessage);
    //    }
    //}

    /// <summary>
    /// Tests that a payout property TheirReference is validated successfully.
    /// </summary>
    [Theory]
    [InlineData("refe-12")]
    [InlineData("r12 hsd-2")]
    [InlineData("-sd73_sdf_48")]
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
}
