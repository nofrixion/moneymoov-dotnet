//-----------------------------------------------------------------------------
// Filename: PaymentRequestExtensionTests.cs
//
// Description: Unit tests for payment request extensions class.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 20 Jul 2023  Arif Matin  Created, Harcourt Street, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using NoFrixion.MoneyMoov.UnitTests;
using Xunit;
using Xunit.Abstractions;

namespace MoneyMoov.UnitTests.Models
{
    public class PaymentRequestExtensionTests
    {
        readonly ILogger<PaymentRequestExtensionTests> _logger;
        private LoggerFactory _loggerFactory;

        public PaymentRequestExtensionTests(ITestOutputHelper testOutputHelper)
        {
            _loggerFactory = new LoggerFactory();
            _loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _logger = _loggerFactory.CreateLogger<PaymentRequestExtensionTests>();
        }

        [Fact]
        public void GetPispPaymentAttempts_FullyPaid_Test()
        {
            var paymentRequestID = Guid.NewGuid();
            var paymentInitiationID = Guid.NewGuid().ToString();
            var paymentServiceProviderID = "bankofireland";
            var amount = 12.12m;

            var pispEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_callback,
                Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent3 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_settle,
                Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvents = new List<PaymentRequestEvent> { pispEvent1, pispEvent2, pispEvent3 };

            var pispAttempts = pispEvents.GetPispPaymentAttempts();

            Assert.NotNull(pispAttempts);
            Assert.NotEmpty(pispAttempts);
            Assert.Single(pispAttempts);
            Assert.Equal(PaymentResultEnum.FullyPaid, pispAttempts.First().Status);
            Assert.Equal(amount, pispAttempts.First().SettledAmount);
            Assert.Equal(amount, pispAttempts.First().AuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, pispAttempts.First().Currency);
            Assert.Equal(PaymentProcessorsEnum.Yapily, pispAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.pisp, pispAttempts.First().PaymentMethod);
        }


        [Fact]
        public void GetPispPaymentAttempts_Authorised_Test()
        {
            var paymentRequestID = Guid.NewGuid();
            var paymentInitiationID = Guid.NewGuid().ToString();
            var paymentServiceProviderID = "bankofireland";
            var amount = 12.12m;

            var pispEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_callback,
                Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvents = new List<PaymentRequestEvent> { pispEvent1, pispEvent2 };

            var pispAttempts = pispEvents.GetPispPaymentAttempts();

            Assert.NotNull(pispAttempts);
            Assert.NotEmpty(pispAttempts);
            Assert.Single(pispAttempts);
            Assert.Equal(PaymentResultEnum.Authorized, pispAttempts.First().Status);
            Assert.Equal(0, pispAttempts.First().SettledAmount);
            Assert.Equal(amount, pispAttempts.First().AuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, pispAttempts.First().Currency);
            Assert.Equal(PaymentProcessorsEnum.Yapily, pispAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.pisp, pispAttempts.First().PaymentMethod);
        }


        [Fact]
        public void GetPispPaymentAttempts_Failed_Test()
        {
            var paymentRequestID = Guid.NewGuid();
            var paymentInitiationID = Guid.NewGuid().ToString();
            var paymentServiceProviderID = "bankofireland";
            var amount = 12.12m;

            var pispEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_callback,
                Status = PaymentRequestResult.PISP_YAPILY_COMPLETED_STATUS,
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent3 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_settle_failure,
                Status = "payment_authorisation_error",
                ErrorMessage = "error",
                ErrorReason = "error",
                PaymentProcessorName = PaymentProcessorsEnum.Yapily,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvents = new List<PaymentRequestEvent> { pispEvent1, pispEvent2, pispEvent3 };

            var pispAttempts = pispEvents.GetPispPaymentAttempts();

            Assert.NotNull(pispAttempts);
            Assert.NotEmpty(pispAttempts);
            Assert.Single(pispAttempts);
            Assert.Equal(PaymentResultEnum.None, pispAttempts.First().Status);
            Assert.Equal(0, pispAttempts.First().SettledAmount);
            Assert.Equal(amount, pispAttempts.First().AuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, pispAttempts.First().Currency);
            Assert.Equal(PaymentProcessorsEnum.Yapily, pispAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.pisp, pispAttempts.First().PaymentMethod);
        }

        [Fact]
        public void GetCardPaymentAttempts_CardSale_FullyPaid_Test()
        {
            var cardAuthorizationResponseID = Guid.NewGuid().ToString();
            var paymentRequestID = Guid.NewGuid();
            var cardRequestID = Guid.NewGuid().ToString();
            var amount = 12.12m;

            var cardEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                Status = "PENDING",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_sale,
                Status = "CAPTURED",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvents = new List<PaymentRequestEvent> { cardEvent1, cardEvent2 };

            var cardAttempts = cardEvents.GetCardPaymentAttempts();

            Assert.NotNull(cardAttempts);
            Assert.NotEmpty(cardAttempts);
            Assert.Single(cardAttempts);
            Assert.Equal(PaymentResultEnum.FullyPaid, cardAttempts.First().Status);
            Assert.Equal(amount, cardAttempts.First().CaptureAttempts.Sum(x => x.CapturedAmount));
            Assert.Equal(amount, cardAttempts.First().CardAuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, cardAttempts.First().Currency);
            Assert.Equal(PaymentProcessorsEnum.Checkout, cardAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.card, cardAttempts.First().PaymentMethod);
        }


        [Fact]
        public void GetCardPaymentAttempts_CardCapture_FullyPaid_Test()
        {
            var cardAuthorizationResponseID = Guid.NewGuid().ToString();
            var paymentRequestID = Guid.NewGuid();
            var cardRequestID = Guid.NewGuid().ToString();
            var amount = 12.12m;

            var cardEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                Status = "PENDING",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_authorization,
                Status = "AUTHORIZED",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvent3 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_capture,
                Status = "CAPTURED",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvents = new List<PaymentRequestEvent> { cardEvent1, cardEvent2, cardEvent3 };

            var cardAttempts = cardEvents.GetCardPaymentAttempts();

            Assert.NotNull(cardAttempts);
            Assert.NotEmpty(cardAttempts);
            Assert.Single(cardAttempts);
            Assert.Equal(PaymentResultEnum.FullyPaid, cardAttempts.First().Status);
            Assert.Equal(amount, cardAttempts.First().CaptureAttempts.Sum(x => x.CapturedAmount));
            Assert.Equal(amount, cardAttempts.First().CardAuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, cardAttempts.First().Currency);
            Assert.Equal(PaymentProcessorsEnum.Checkout, cardAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.card, cardAttempts.First().PaymentMethod);
        }


        [Fact]
        public void GetCardPaymentAttempts_CardFailure_Test()
        {
            var cardAuthorizationResponseID = Guid.NewGuid().ToString();
            var paymentRequestID = Guid.NewGuid();
            var cardRequestID = Guid.NewGuid().ToString();
            var amount = 12.12m;

            var cardEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_payer_authentication_setup,
                Status = "PENDING",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.card_payer_authentication_failure,
                Status = "DECLINED",
                PaymentProcessorName = PaymentProcessorsEnum.Checkout,
                CardAuthorizationResponseID = cardAuthorizationResponseID,
                CardRequestID = cardRequestID,
            };

            var cardEvents = new List<PaymentRequestEvent> { cardEvent1, cardEvent2 };

            var cardAttempts = cardEvents.GetCardPaymentAttempts();

            Assert.NotNull(cardAttempts);
            Assert.NotEmpty(cardAttempts);
            Assert.Equal(2, cardAttempts.Count());
            Assert.Equal(PaymentResultEnum.None, cardAttempts.First().Status);
            Assert.Equal(0, cardAttempts.First().SettledAmount);
            Assert.Equal(0, cardAttempts.First().AuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, cardAttempts.First().Currency);
            Assert.Equal(PaymentResultEnum.None, cardAttempts.Last().Status);
            Assert.NotNull(cardAttempts.Last().CardPayerAuthenticationSetupFailedAt);
        }


        [Theory]
        [InlineData(PaymentRequestResult.PISP_YAPILY_AUTHORISATION_ERROR, PaymentProcessorsEnum.Yapily)]
        [InlineData(PaymentRequestResult.PISP_NOFRIXION_AUTHORISATION_ERROR, PaymentProcessorsEnum.Nofrixion)]
        public void GetPispPaymentAttempts_Bank_Authorisation_Failure(string status, PaymentProcessorsEnum paymentProcessor)
        {
            var paymentRequestID = Guid.NewGuid();
            var paymentInitiationID = Guid.NewGuid().ToString();
            var paymentServiceProviderID = "bankofireland";
            var amount = 12.12m;

            var pispEvent1 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_initiate,
                Status = PaymentRequestResult.PISP_YAPILY_PENDING_STATUS,
                PaymentProcessorName = paymentProcessor,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvent2 = new PaymentRequestEvent
            {
                ID = Guid.NewGuid(),
                PaymentRequestID = paymentRequestID,
                Amount = amount,
                Currency = CurrencyTypeEnum.EUR,
                Inserted = DateTime.UtcNow,
                EventType = PaymentRequestEventTypesEnum.pisp_callback,
                Status = status,
                PaymentProcessorName = paymentProcessor,
                PispPaymentInitiationID = paymentInitiationID,
                PispPaymentServiceProviderID = paymentServiceProviderID,
                PispPaymentInstitutionName = "Bank of Ireland"
            };

            var pispEvents = new List<PaymentRequestEvent> { pispEvent1, pispEvent2 };

            var pispAttempts = pispEvents.GetPispPaymentAttempts();

            Assert.NotNull(pispAttempts);
            Assert.NotEmpty(pispAttempts);
            Assert.Single(pispAttempts);
            Assert.Equal(PaymentResultEnum.None, pispAttempts.First().Status);
            Assert.Equal(0, pispAttempts.First().SettledAmount);
            Assert.Equal(0, pispAttempts.First().AuthorisedAmount);
            Assert.Equal(CurrencyTypeEnum.EUR, pispAttempts.First().Currency);
            Assert.Equal(paymentProcessor, pispAttempts.First().PaymentProcessor);
            Assert.Equal(PaymentMethodTypeEnum.pisp, pispAttempts.First().PaymentMethod);
            Assert.NotNull(pispAttempts.First().PispAuthorisationFailedAt);
        }
    }
}
