// -----------------------------------------------------------------------------
//  Filename: PaymentRequestPaymentAttemptTests.cs
// 
//  Description: Unit tests for the MoneyMoov PaymentRequestPaymentAttempt model.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  17 08 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace MoneyMoov.UnitTests.Models;

public class PaymentRequestPaymentAttemptTests
{
    [Fact]
        public void PaymentRequestPaymentAttempt_ReconciledTransactionID_Test()
        {
            var paymentRequestID = Guid.NewGuid();
            var paymentInitiationID = Guid.NewGuid().ToString();
            var paymentServiceProviderID = "bankofireland";
            var amount = 12.12m;
            var reconciledTransactionID = Guid.NewGuid();

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
                PispPaymentInstitutionName = "Bank of Ireland",
                ReconciledTransactionID = reconciledTransactionID
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
            Assert.Equal(reconciledTransactionID, pispAttempts.First().ReconciledTransactionID);
        }
}