//-----------------------------------------------------------------------------
// Filename: PaymentRequestCreateTests.cs
//
// Description: Unit tests for payment request create validation logic.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 07 May 2022  Aaron Clauson  Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PaymentRequestCreateTests
{
    readonly ILogger<PaymentRequestCreateTests> _logger;
    private LoggerFactory _loggerFactory;

    public PaymentRequestCreateTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PaymentRequestCreateTests>();
    }

    /// <summary>
    /// Tests that a payment request create model can be created with only the mandatory fields.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Mandatory_Fields_Only()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost"
        };

        Assert.NotNull(paymentRequest);

        var validationProb = paymentRequest.Validate();

        Assert.True(validationProb.IsEmpty);
    }

    /// <summary>
    /// Tests that a payment request create model with a valid PISP recipient reference is identified
    /// as being valid.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Valid_Pisp_Recipient_Reference()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost",
            PispRecipientReference = "abc 123"
        };

        Assert.NotNull(paymentRequest);

        var validationProb = paymentRequest.Validate();

        Assert.True(validationProb.IsEmpty);
    }

    /// <summary>
    /// Tests that a payment request create model with an invalid character in the PISP recipient reference 
    /// is identified as being invalid.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Invalid_Pisp_Recipient_Reference()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost",
            PispRecipientReference = "abc-123 !"
        };

        Assert.NotNull(paymentRequest);

        var validationProb = paymentRequest.Validate();

        Assert.False(validationProb.IsEmpty);
        Assert.Single(validationProb.Errors);
        Assert.True(validationProb.Errors.ContainsKey("PispRecipientReference"));
    }

    /// <summary>
    /// Tests that a create payment request model gets the expected validation error when
    /// the payment request callback URL is malformed.
    /// </summary>
    [Fact]
    public void Create_Card_PaymentRequest_Malformed_CallbackUrl_Invalid()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "not a url",
            BaseOriginUrl = "https://localhost"
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Contains(validationResults.Single().MemberNames, x => x == nameof(paymentRequestCreate.CallbackUrl));
        Assert.Contains("not recognised as a valid URL", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Tests that a create payment request model is successful when the payment request 
    /// base origin URL is not supplied. Note that on 28 Oct 2022 the logic that previously
    /// specified the BaseOriginUrl as being mandatory was changed to only require it if
    /// the merchant's card processor was CyberSource or was not specified.
    /// </summary>
    [Fact]
    public void Create_Card_PaymentRequest_Empty_OriginUrl_Success()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost"
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.Empty(validationResults);
    }

    /// <summary>
    /// Tests that a create payment request model gets the expected validation error when
    /// the origin URL is malformed.
    /// </summary>
    [Fact]
    public void Create_Card_PaymentRequest_Malformed_OriginUrl_Invalid()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost",
            BaseOriginUrl = "not a url"
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Contains(validationResults.Single().MemberNames, x => x == nameof(paymentRequestCreate.BaseOriginUrl));
        Assert.Contains("not recognised as a valid URL", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Tests that a create payment request model gets the expected validation error when
    /// the origin URL has extra segments (CyberSource require the URL to have no path segments).
    /// </summary>
    [Fact]
    public void Create_Card_PaymentRequest_ExtraSegments_OriginUrl_Invalid()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/good",
            BaseOriginUrl = "https://localhost/bad"
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Contains(validationResults.Single().MemberNames, x => x == nameof(paymentRequestCreate.BaseOriginUrl));
        Assert.Contains("had extra segments", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Tests that a create payment request model gets the expected validation error when
    /// the amount is set to zero with payment methods including an option other than card.
    /// </summary>
    [Fact]
    public void Create_PaymentRequest_Zero_Amount_Failure()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 0.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/good",
            BaseOriginUrl = "https://localhost",
            PaymentMethodTypes = PaymentMethodTypeEnum.card | PaymentMethodTypeEnum.lightning
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Contains(validationResults.Single().MemberNames, x => x == nameof(paymentRequestCreate.Amount));
        Assert.Contains("The amount can only be set to 0 when the payment methods are set to a single option of card.", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Tests that a create payment request model is accepted when
    /// the amount is set to zero with the required options.
    /// </summary>
    [Fact]
    public void Create_PaymentRequest_Zero_Amount_Success()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 0.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/good",
            BaseOriginUrl = "https://localhost",
            CardAuthorizeOnly = true
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.Empty(validationResults);
    }

    /// <summary>
    /// Tests that a create payment request model gets the expected validation error when
    /// the base origin URL does not specify an https URL.
    /// </summary>
    [Fact]
    public void Create_Card_PaymentRequest_NonHttps_BaseOriginUrl_Invalid()
    {
        var paymentRequestCreate = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/good",
            BaseOriginUrl = "http://localhost"
        };

        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(paymentRequestCreate, context);

        Assert.NotNull(validationResults);
        Assert.Single(validationResults);
        Assert.Contains(validationResults.Single().MemberNames, x => x == nameof(paymentRequestCreate.BaseOriginUrl));
        Assert.Contains("must be an https URL", validationResults.Single().ErrorMessage);
    }

    /// <summary>
    /// Tests that a payment request create model returns a validation error if the amount specified for
    /// a EUR PIS attempt is too low.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Amount_Too_Low_EUR_PIS()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 0.99M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            PaymentMethodTypes = PaymentMethodTypeEnum.pisp
        };

        var validationProb = paymentRequest.Validate();

        Assert.False(validationProb.IsEmpty);
        Assert.Contains("the amount must be at least EUR", validationProb.Errors.Single().Value.Single());
    }

    /// <summary>
    /// Tests that a payment request create model is validated correctly with the minimum EUR PIS amount.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_EUR_PIS()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 1.00M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            PaymentMethodTypes = PaymentMethodTypeEnum.pisp
        };

        var validationProb = paymentRequest.Validate();

        Assert.True(validationProb.IsEmpty);
    }

    /// <summary>
    /// Tests that a payment request create model returns a validation error if the amount specified for
    /// a GBP PIS attempt is too low.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Amount_Too_Low_GBP_PIS()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 1.99M,
            Currency = CurrencyTypeEnum.GBP,
            CallbackUrl = "https://localhost/callback",
            PaymentMethodTypes = PaymentMethodTypeEnum.pisp
        };

        var validationProb = paymentRequest.Validate();

        Assert.False(validationProb.IsEmpty);
        Assert.Contains("the amount must be at least GBP", validationProb.Errors.Single().Value.Single());
    }

    /// <summary>
    /// Tests that a payment request create model is validated correctly with the minimum GBP PIS amount.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_GBP_PIS()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 2.00M,
            Currency = CurrencyTypeEnum.GBP,
            CallbackUrl = "https://localhost/callback",
            PaymentMethodTypes = PaymentMethodTypeEnum.pisp
        };

        var validationProb = paymentRequest.Validate();

        Assert.True(validationProb.IsEmpty);
    }

    /// <summary>
    /// Tests that a payment request create model is validated when a card token is requested and the
    /// customer email address is set.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Card_Token_With_Customer_Email()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 0.01M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost",
            PaymentMethodTypes = PaymentMethodTypeEnum.card,
            CardCreateToken = true,
            CardCreateTokenMode = CardTokenCreateModes.ConsentNotRequired,
            CustomerEmailAddress = "qa@nofrixion.com"
        };

        var validationProb = paymentRequest.Validate();

        Assert.True(validationProb.IsEmpty);
    }

    /// <summary>
    /// Tests that a payment request create model returns a validation error when a card token is requested but no 
    /// customer email address is supplied.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Card_Token_But_No_Customer_Email()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 0.01M,
            Currency = CurrencyTypeEnum.GBP,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost",
            PaymentMethodTypes = PaymentMethodTypeEnum.card,
            CardCreateToken = true
        };

        var validationProb = paymentRequest.Validate();

        Assert.False(validationProb.IsEmpty);
        Assert.Contains("The CustomerEmailAddress must be set when the CardCreateToken is set.", validationProb.Errors.Single().Value.Single());
    }

    /// <summary>
    /// Tests that a payment request create model returns a validation error when a card token is requested and an
    /// invalid customer email address is supplied.
    /// </summary>
    [Fact]
    public void Create_PaymentRequestCreate_Card_Token_With_Invalid_Customer_Email()
    {
        var paymentRequest = new PaymentRequestCreate
        {
            Amount = 0.01M,
            Currency = CurrencyTypeEnum.EUR,
            CallbackUrl = "https://localhost/callback",
            BaseOriginUrl = "https://localhost",
            PaymentMethodTypes = PaymentMethodTypeEnum.card,
            CardCreateToken = true,
            CustomerEmailAddress = "qa-nofrixion.com"
        };

        var validationProb = paymentRequest.Validate();

        Assert.False(validationProb.IsEmpty);
        Assert.Contains("The CustomerEmailAddress field is not a valid e-mail address.", validationProb.Errors.Single().Value.Single());
    }
}