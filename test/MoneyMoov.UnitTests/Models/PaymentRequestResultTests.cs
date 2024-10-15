//-----------------------------------------------------------------------------
// Filename: PaymentRequestResultTests.cs
//
// Description: Unit tests for payment request result class.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Mar 2022  Aaron Clauson  Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class PaymentRequestResultTests
{
    readonly ILogger<PaymentRequestResultTests> _logger;
    private LoggerFactory _loggerFactory;

    public PaymentRequestResultTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = new LoggerFactory();
        _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        _logger = _loggerFactory.CreateLogger<PaymentRequestResultTests>();
    }

    /// <summary>
    /// Tests that a fully paid payment request is generated correctly from a payment request
    /// and its events.
    /// </summary>
    [Fact]
    public void Fully_Paid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = Guid.NewGuid().ToString()
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that the expected result gets returned if the payment request the result is
    /// being retrieved for does not have any events.
    /// </summary>
    [Fact]
    public void No_PaymentRequest_Events_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var result = new PaymentRequestResult(entity);

        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    /// <summary>
    /// Tests that an underpaid payment request returns a partially paid result.
    /// </summary>
    [Fact]
    public void Underpaid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = "dummy"
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount / 2, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
    }

    /// <summary>
    /// Tests that an overpaid payment request returns an overpaid result.
    /// </summary>
    [Fact]
    public void Overpaid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount * 2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = Guid.NewGuid().ToString()
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount * 2, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.OverPaid, result.Result);
    }

    /// <summary>
    /// Tests that an voided payment request returns a voided result.
    /// </summary>
    [Fact]
    public void Voided_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = "1234567"
        };

        var voidEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_void,
            Status = CardPaymentResponseStatus.CARD_VOIDED_SUCCESS_STATUS,
            CardAuthorizationResponseID = "1234567"
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent, voidEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.Voided, result.Result);
    }

    /// <summary>
    /// Tests that multiple payment with one getting voided returns a fully paid result.
    /// </summary>
    [Fact]
    public void Multiple_With_Void_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = "1234567"
        };

        var voidEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_void,
            Status = CardPaymentResponseStatus.CARD_VOIDED_SUCCESS_STATUS,
            CardAuthorizationResponseID = "1234567"
        };

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = string.Empty,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            PispPaymentInitiationID = "xxx123"
        };

        var pispEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = string.Empty,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            PispPaymentInitiationID = "xxx123"
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent, voidEvent, initiateEvent, pispEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that a lightning payment returns a fully paid result.
    /// </summary>
    [Fact]
    public void Lightning_Fully_Paid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.lightning_invoice_paid
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that a card payment with a soft decline returns a fully paid result.
    /// </summary>
    [Fact]
    public void Card_Soft_Decline_Fully_Paid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();

        var successEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_sale,
            Status = CardPaymentResponseStatus.CARD_PAYMENT_SOFT_DECLINE_STATUS,
            CardAuthorizationResponseID = Guid.NewGuid().ToString()
        };

        entity.Events = new List<PaymentRequestEvent> { successEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
        Assert.Equal(0, result.Payments.Single().CardCapturedAmount);
    }

    /// <summary>
    /// Tests that a card payment done in two stages with an authorisation followed by a 
    /// capture returns a fully paid result.
    /// </summary>
    [Fact]
    public void Card_Authorise_And_Capture_Fully_Paid_Payment_Result()
    {
        var entity = GetTestPaymentRequest();
        entity.CardAuthorizeOnly = true;

        var cardAuthorizationResponseID = Guid.NewGuid().ToString();
        var authoriseEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_authorization,
            Status = CardPaymentResponseStatus.CARD_AUTHORIZED_SUCCESS_STATUS,
            CardAuthorizationResponseID = cardAuthorizationResponseID
        };

        var captureEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_capture,
            Status = CardPaymentResponseStatus.CARD_CAPTURE_SUCCESS_STATUS,
            CardAuthorizationResponseID = cardAuthorizationResponseID
        };

        entity.Events = new List<PaymentRequestEvent> { authoriseEvent, captureEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
        Assert.Equal(entity.Amount, result.Payments.Last().CardCapturedAmount);
    }

    /// <summary>
    /// Tests that a payment request with a single payment initiated event return a none result.
    /// </summary>
    [Fact]
    public void Pisp_initiate_Only_Result()
    {
        var entity = GetTestPaymentRequest();

        var pispEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_PLAID_INITIATED_STATUS
        };

        entity.Events = new List<PaymentRequestEvent> { pispEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(decimal.Zero, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    private PaymentRequest GetTestPaymentRequest()
    {
        var id = Guid.NewGuid();
        var merchantID = Guid.NewGuid();
        var amount = 0.42M;
        var currency = CurrencyTypeEnum.EUR;
        var paymentMethods = new List<PaymentMethodTypeEnum> { PaymentMethodTypeEnum.card, PaymentMethodTypeEnum.lightning };
        var description = "desc";
        var customerID = "cid";
        var orderID = "oid";
        var pispAccountID = Guid.NewGuid();
        var baseOriginUrl = "https://localhost";
        var callbackUrl = "https://localhost";

        var entity = new PaymentRequest
        {
            ID = id,
            MerchantID = merchantID,
            Amount = amount,
            Currency = currency,
            PaymentMethods = paymentMethods,
            Description = description,
            CustomerID = customerID,
            OrderID = orderID,
            PispAccountID = pispAccountID,
            BaseOriginUrl = baseOriginUrl,
            CallbackUrl = callbackUrl
        };

        return entity;
    }

    /// <summary>
    /// Tests that multiple pisp successful authorizations returns an Authorized result.
    /// </summary>
    [Fact]
    public void Multiple_Pisp_Success_Events_Authorized_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        var yapilyCompletedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = "COMPLETED",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            PispPaymentInitiationID = "yapily123"
        };
        var plaidExecutedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_PLAID_SUCCESS_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Plaid,
            PispPaymentInitiationID = "plaid123"
        };
        var yapilyCompletedWebhookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            PispPaymentInitiationID = "yapily123"
        };

        entity.Events = new List<PaymentRequestEvent> { yapilyCompletedEvent, plaidExecutedEvent, yapilyCompletedWebhookEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.Authorized, result.Result);
    }

    /// <summary>
    /// Tests that 3 pisp authorization events and 1 pisp settle event results in Partially Paid status.
    /// </summary>
    [Fact]
    public void Multiple_Pisp_Success_Events_With_One_Settle_Event_PartiallyPaid_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        entity.Amount = 100;

        var yapilyCompletedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/3,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = "COMPLETED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var plaidExecutedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/3,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_PLAID_SUCCESS_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var yapilyCompletedWebhookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/3,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var pisSettleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/3,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = PaymentsConstants.PISP_SETTLED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { yapilyCompletedEvent, plaidExecutedEvent, yapilyCompletedWebhookEvent, pisSettleEvent };
        var result = new PaymentRequestResult(entity);

        Assert.Equal(Math.Round(entity.Amount / 3, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.Amount);
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
    }

    [Fact]
    public void Mixed_Payment_Events_Check_Outstanding_Amount()
    {
        var entity = GetTestPaymentRequest();
        var pispPaymentInititionID1 = "id1";
        var pispPaymentInititionID2 = "id2";

        entity.Amount = 120;

        var captureEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.card_capture,
            Status = CardPaymentResponseStatus.CARD_CAPTURE_SUCCESS_STATUS
        };

        var yapilyCompletedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "COMPLETED",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var plaidExecutedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            PispPaymentInitiationID = pispPaymentInititionID2,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_PLAID_SUCCESS_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Plaid
        };
        var yapilyCompletedWebhookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            PispPaymentInitiationID = pispPaymentInititionID1,
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var pisSettleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = PaymentsConstants.PISP_SETTLED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { yapilyCompletedEvent, plaidExecutedEvent, yapilyCompletedWebhookEvent, pisSettleEvent };
        var result = new PaymentRequestResult(entity);

        Assert.Equal(Math.Round(entity.Amount / 4, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.Amount);
        Assert.Equal(Math.Round(entity.Amount / 4, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.PispAmountAuthorized());
        Assert.Equal(Math.Round((entity.Amount / 4) * 2, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
    }

    [Fact]
    public void Payment_Events_With_Zero_Amount_Check_Outstanding_Amount()
    {
        var entity = GetTestPaymentRequest();
        var pispPaymentInititionID1 = "id1";

        entity.Amount = 120;

        var yapilyInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow-new TimeSpan(0, 0, 3),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "COMPLETED",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var yapilyCompletedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 2),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "COMPLETED",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };
        var yapilyCompletedWebhookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = decimal.Zero,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            PispPaymentInitiationID = pispPaymentInititionID1,
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { yapilyCompletedWebhookEvent, yapilyInitiatedEvent, yapilyCompletedEvent };
        var result = new PaymentRequestResult(entity);

        Assert.Equal(Math.Round(decimal.Zero, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.Amount);
        Assert.Equal(Math.Round(entity.Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.PispAmountAuthorized());
        Assert.Equal(Math.Round(decimal.Zero, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.Authorized, result.Result);
    }

    /// <summary>
    /// Checks that multiple PIS attempts and failures does not cause the payment request status to get updated to authorized.
    /// </summary>
    [Fact]
    public void Payment_Events_Mulitple_PIS_Failures_NonAuthorized_Status()
    {
        var entity = GetTestPaymentRequest();
        var pispPaymentInititionID1 = "id1";

        entity.Amount = 120;

        var yapilyInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 3),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "AWAITINGAUTHORIZATION"
        };

        var yapilyAuthorisationErrorEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 2),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "payment_authorisation_error"
        };

        var plaidInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 3),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = string.Empty
        };

        var plaidWebHookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = 0,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 3),
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "PaymentStatusInputNeeded"
        };

        entity.Events = new List<PaymentRequestEvent> { plaidWebHookEvent, plaidInitiatedEvent, yapilyInitiatedEvent, yapilyAuthorisationErrorEvent };
        var result = new PaymentRequestResult(entity);

        Assert.Equal(Math.Round(decimal.Zero, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.Amount);
        Assert.Equal(Math.Round(decimal.Zero, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.PispAmountAuthorized());
        Assert.Equal(Math.Round(entity.Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES), result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    /// <summary>
    /// Checks that a callback event arriving after the funds have been received does not revert the fully paid status.
    /// </summary>
    [Fact]
    public void Payment_Events_Fully_Paid_Not_Reverted()
    {
        var entity = GetTestPaymentRequest();
        var pispPaymentInititionID1 = "id1";
        var now = DateTime.UtcNow;

        entity.Amount = 120;

        var yapilyInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = now - new TimeSpan(0, 0, 10),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "AWAITINGAUTHORIZATION",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events.Add(yapilyInitiatedEvent);
        var result = new PaymentRequestResult(entity);
        Assert.Equal(PaymentResultEnum.None, result.Result);

        var yapilyCallbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = now - new TimeSpan(0, 0, 9),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = string.Empty,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events.Add(yapilyCallbackEvent);
        result = new PaymentRequestResult(entity);
        Assert.Equal(PaymentResultEnum.None, result.Result);

        var yapilyCallbackEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = now - new TimeSpan(0, 0, 8),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events.Add(yapilyCallbackEvent2);
        result = new PaymentRequestResult(entity);
        Assert.Equal(PaymentResultEnum.Authorized, result.Result);

        var settledEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow - new TimeSpan(0, 0, 7),
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = "SETTLED"
        };

        entity.Events.Add(settledEvent);
        result = new PaymentRequestResult(entity);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);

        var yapilyWebhookEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = now - new TimeSpan(0, 0, 6),
            EventType = PaymentRequestEventTypesEnum.pisp_webhook,
            PispPaymentInitiationID = pispPaymentInititionID1,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events.Add(yapilyWebhookEvent);
        result = new PaymentRequestResult(entity);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(decimal.Zero, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that multiple pisp successful authorizations returns an Authorized result.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Success_Events_Authorized_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        var nofrixionPendingEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PayoutStatus.PENDING.ToString(),
            PaymentProcessorName = PaymentProcessorsEnum.Nofrixion,
            PispPaymentInitiationID = "xxx"
        };

        var nofrixionQueuedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PayoutStatus.QUEUED.ToString(),
            PaymentProcessorName = PaymentProcessorsEnum.Nofrixion,
            PispPaymentInitiationID = "xxx"
        };

        entity.Events = new List<PaymentRequestEvent> { nofrixionPendingEvent, nofrixionQueuedEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(entity.Amount, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.Authorized, result.Result);
    }

    /// <summary>
    /// Tests that multiple pisp successful authorizations with a settle returns a FullyPaid result.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Success_And_Settle_Events_FullyPaid_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        var nofrixionPendingEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PayoutStatus.PENDING.ToString(),
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var nofrixionQueuedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PayoutStatus.QUEUED.ToString(),
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { nofrixionPendingEvent, nofrixionQueuedEvent, settleEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that if the executed callback arrives AFTER the PIS payment has already settled
    /// the status gets set as FullyPaid, not Authorized.
    /// </summary>
    [Fact]
    public void Multiple_Pisp_Authorise_After_Settle_Authorized_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = "COMPLETED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settlementEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddSeconds(1),
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = PaymentsConstants.PISP_SETTLED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        // Typically the callback arrives after settlement but in some cases on SEPA_INST or FasterPayments
        // settlement can occur faster than the callback.
        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddSeconds(2),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_MODULR_SUCCESS_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { initiateEvent, settlementEvent, callbackEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that if pisp attempts is Authorized but then the funds don't arrive and
    /// a settlement failure event is recorded the status is set back to None.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Settlment_Failure_None_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        string pispPaymentID = "xxx";

        var pispInitiate = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddDays(-5),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddDays(-4),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        var setlementFailedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle_failure,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            Status = PaymentsConstants.PISP_FAILED_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        entity.Events = new List<PaymentRequestEvent> { pispInitiate, callbackEvent, setlementFailedEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    /// <summary>
    /// Tests that if pisp attempts is Authorized, funds don't arrive resulting in a settlement failure
    /// event and then funds do subsequently arrive the status is correctly set to FullyPaid.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Settlment_Overrules_Failure_FullyPaid_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        string pispPaymentID = "xxx";

        var pispInitiate = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddDays(-5),
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow.AddDays(-4),
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        var setlementFailedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle_failure,
            Status = PaymentsConstants.PISP_FAILED_STATUS,
            PispPaymentInitiationID = pispPaymentID
        };

        var settlementEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = PaymentsConstants.PISP_SETTLED_STATUS,
            PispPaymentInitiationID = pispPaymentID,
            Amount = entity.Amount,
            Currency = entity.Currency
        };

        entity.Events = new List<PaymentRequestEvent> { pispInitiate, callbackEvent, setlementFailedEvent, settlementEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that an orphaned (no related initiate or callback events) still results in a 
    /// None result. Settlements with an empty PispPaymentInitiationID should only ever occur 
    /// if something has gone very wrong and will need to be manually handled.
    /// </summary>
    [Fact]
    public void Pisp_Settlement_With_No_PispPaymentInitiationID_Ignored()
    {
        var entity = GetTestPaymentRequest();

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED"
        };

        entity.Events = new List<PaymentRequestEvent> { settleEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    /// <summary>
    /// Tests that a pisp attempt ignores duplicate settlement events and results
    /// in a fully paid status.
    /// </summary>
    [Fact]
    public void Duplicate_Settlements_Igrnored_FullyPaid()
    {
        var entity = GetTestPaymentRequest();

        var initiate = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PayoutStatus.PENDING.ToString(),
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callback = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PayoutStatus.QUEUED.ToString(),
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleDuplicateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { initiate, callback, settleEvent, settleDuplicateEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that if the Modulr event has a bank rejected response it does not result in the 
    /// status being set to Authorized.
    /// </summary>
    [Fact]
    public void Modulr_Bank_Reject_Not_Authorized_Result()
    {
        var entity = GetTestPaymentRequest();

        var modulrInitiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PayoutStatus.PENDING.ToString(),
            PaymentProcessorName = PaymentProcessorsEnum.Modulr,
            PispPaymentInitiationID = "xxx"
        };

        var modulrCallbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_MODULR_SUCCESS_STATUS,
            PaymentProcessorName = PaymentProcessorsEnum.Modulr,
            PispPaymentInitiationID = "xxx",
            PispBankStatus = PaymentRequestResult.PISP_MODULR_BANK_REJECTED_STATUS
        };

        entity.Events = new List<PaymentRequestEvent> { modulrInitiateEvent, modulrCallbackEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }

    /// <summary>
    /// Tests that two separate pay by bank attempts where the first one is abandoned results in
    /// a fully paid result.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_First_Attempt_Abandoned_FullyPaid_Result_Status()
    {
        var entity = GetTestPaymentRequest();

        // Payer click on pay by bank but then stops and goes back to try another bank.
        var initiateOnlyEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { initiateOnlyEvent, initiateEvent, callbackEvent, settleEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(decimal.Zero, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.FullyPaid, result.Result);
    }

    /// <summary>
    /// Tests that two separate pay by bank attempts where the first one is authorised and the second one
    /// is settled resutls in the correct status.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Multiple_Attempts_Autorised_And_Settled()
    {
        var entity = GetTestPaymentRequest();

        var callbackAuthoriseEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount / 2 ,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { callbackAuthoriseEvent, initiateEvent, callbackEvent, settleEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount / 2, result.Amount);
        Assert.Equal(entity.Amount / 2, result.PispAmountAuthorized());
        Assert.Equal(0, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
    }

    /// <summary>
    /// Tests that two separate pay by bank attempts are successful and results in overpaid.
    /// </summary>
    [Fact]
    public void Nofrixion_Pisp_Multiple_Attempts_OverPaid()
    {
        var entity = GetTestPaymentRequest();

        var callbackEvent1 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent1 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        entity.Events = new List<PaymentRequestEvent> { callbackEvent1, settleEvent1, initiateEvent, callbackEvent, settleEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount * 2, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(0, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.OverPaid, result.Result);
    }
    
    [Fact]
    public void Nofrixion_Pisp_Single_Attempt_Full_Refund_OutstandingAmount_Equal_To_InitalAmount()
    {
        var entity = GetTestPaymentRequest();

        var refundPayoutID = Guid.NewGuid();

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent = new PaymentRequestEvent
                              {
                                  ID = Guid.NewGuid(),
                                  PaymentRequestID = entity.ID,
                                  Amount = entity.Amount,
                                  Currency = entity.Currency,
                                  Inserted = DateTime.UtcNow,
                                  EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
                                  Status = "QUEUED",
                                  PispPaymentInitiationID = "xxx",
                                  PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                                    RefundPayoutID = refundPayoutID
                              };

        var refundSettledEvent = new PaymentRequestEvent
                              {
                                  ID = Guid.NewGuid(),
                                  PaymentRequestID = entity.ID,
                                  Amount = entity.Amount,
                                  Currency = entity.Currency,
                                  Inserted = DateTime.UtcNow,
                                  EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
                                  Status = "PROCESSED",
                                  PispPaymentInitiationID = "xxx",
                                  PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                                    RefundPayoutID = refundPayoutID
                              };

        entity.Events = new List<PaymentRequestEvent> { initiateEvent, callbackEvent, settleEvent, refundInitiatedEvent, refundSettledEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(Decimal.Zero, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }
    
    [Fact]
    public void Nofrixion_Pisp_Single_Attempt_Partial_Refund()
    {
        var entity = GetTestPaymentRequest();
        
        entity.Amount = 100;

        var refundPayoutID = Guid.NewGuid();

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
            Status = "QUEUED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };

        var refundSettledEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
            Status = "PROCESSED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };

        entity.Events = new List<PaymentRequestEvent> { initiateEvent, callbackEvent, settleEvent, refundInitiatedEvent, refundSettledEvent };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount/2, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount/2, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
        Assert.Equal(entity.Amount/2, result.Payments.Single().RefundedAmount);
    }
    
    [Fact]
    public void Nofrixion_Pisp_Multiple_Attempts_Multiple_Single_Refunds()
    {
        var entity = GetTestPaymentRequest();
        
        entity.Amount = 100;

        var refundPayoutID = Guid.NewGuid();
        var refundPayoutID2 = Guid.NewGuid();

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
            Status = "QUEUED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };

        var refundSettledEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
            Status = "PROCESSED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };
        
        var initiateEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
            Status = "QUEUED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID2
        };

        var refundSettledEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
            Status = "PROCESSED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID2
        };

        entity.Events = new List<PaymentRequestEvent> { initiateEvent, callbackEvent, settleEvent, refundInitiatedEvent, refundSettledEvent, initiateEvent2, callbackEvent2, settleEvent2, refundInitiatedEvent2, refundSettledEvent2 };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(0, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.None, result.Result);
    }
    
    [Fact]
    public void Nofrixion_Pisp_Multiple_Attempts_Multiple_Partial_Refunds()
    {
        var entity = GetTestPaymentRequest();
        
        entity.Amount = 100;

        var refundPayoutID = Guid.NewGuid();
        var refundPayoutID2 = Guid.NewGuid();

        var initiateEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
            Status = "QUEUED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };

        var refundSettledEvent = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
            Status = "PROCESSED",
            PispPaymentInitiationID = "xxx",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID
        };
        
        var initiateEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_initiate,
            Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var callbackEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_callback,
            Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var settleEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/2,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_settle,
            Status = "SETTLED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily
        };

        var refundInitiatedEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_initiated,
            Status = "QUEUED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID2
        };

        var refundSettledEvent2 = new PaymentRequestEvent
        {
            ID = Guid.NewGuid(),
            PaymentRequestID = entity.ID,
            Amount = entity.Amount/4,
            Currency = entity.Currency,
            Inserted = DateTime.UtcNow,
            EventType = PaymentRequestEventTypesEnum.pisp_refund_settled,
            Status = "PROCESSED",
            PispPaymentInitiationID = "yyy",
            PaymentProcessorName = PaymentProcessorsEnum.Yapily,
            RefundPayoutID = refundPayoutID2
        };

        entity.Events = new List<PaymentRequestEvent> { initiateEvent, callbackEvent, settleEvent, refundInitiatedEvent, refundSettledEvent, initiateEvent2, callbackEvent2, settleEvent2, refundInitiatedEvent2, refundSettledEvent2 };

        var result = new PaymentRequestResult(entity);

        Assert.Equal(entity.Amount/2, result.Amount);
        Assert.Equal(0, result.PispAmountAuthorized());
        Assert.Equal(entity.Amount/2, result.AmountOutstanding());
        Assert.Equal(entity.Currency, result.Currency);
        Assert.Equal(PaymentResultEnum.PartiallyPaid, result.Result);
        Assert.Equal(entity.Amount/2, result.Payments.Sum(x=>x.RefundedAmount));
    }
}