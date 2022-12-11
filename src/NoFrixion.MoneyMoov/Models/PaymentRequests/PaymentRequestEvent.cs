//-----------------------------------------------------------------------------
// Filename: PaymentRequestEvent.cs
//
// Description: Represents an event, including a payment authorisation, for a
// payment request.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 14 Dec 2021  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 30 Dec 2021  Aaron Clauson   Renamed from PaymentReceive to PaymentRequestEvent.
// 17 Jan 2022  Aaron Clauson   Renamed from PaymentRequestEvent to PaymentRequestCardEvent.
// 12 Feb 2022  Aaron Clauson   Renamed back to PaymentRequestEvent.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestEvent
{
    public Guid ID { get; set; }

    public Guid PaymentRequestID { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public PaymentRequestEventTypesEnum EventType { get; set; } = PaymentRequestEventTypesEnum.unknown;

    [Required]
    public decimal Amount { get; set; }

    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    public string? Status { get; set; }

    public string? ErrorReason { get; set; }

    public string? ErrorMessage { get; set; }

    [JsonIgnore]
    public string? RawResponse { get; set; }

    [JsonIgnore]
    public string? RawResponseHash { get; set; }

    public string? CardRequestID { get; set; }

    public string? CardTransactionID { get; set; }

    /// <summary>
    /// If the option to create a reusable token for card payments was set this field contains
    /// the token the merchant can store to use for repeat payments.
    /// </summary>
    public string? CardTokenCustomerID { get; set; }

    /// <summary>
    /// For a successful card authorization this field will hold the response ID. If a capture
    /// needs to be performed this is the ID that must be used.
    /// </summary>
    public string? CardAuthorizationResponseID { get; set; }

    /// <summary>
    /// For Bitcoin Lightning payments this filed holds the invoice presented to the payer.
    /// </summary>
    public string? LightningInvoice { get; set; }

    /// <summary>
    /// For a payment initiation this is the service provider ID selected by the payer, typically
    /// the ID for the bank or similar financial institution.
    /// </summary>
    public string? PispPaymentServiceProviderID { get; set; }

    /// <summary>
    /// For a payment initiation this is the ID returned by the service provider initiating
    /// the payment for us.
    /// </summary>
    public string? PispPaymentInitiationID { get; set; }

    /// <summary>
    /// For a payment initiation this is the redirect URL returned by the service provider
    /// initiating the payment for us. This is the URL the payer is redirected to 
    /// authenticate with their financial institution and authorise the payment.
    /// </summary>
    public string? PispRedirectUrl { get; set; }

    /// <summary>
    /// For payment initiation providers that use an OAuth, or other, token to create a session
    /// between submitting and finalising a payment initiation attempt.
    /// </summary>
    public string? PispToken { get; set; }

    /// <summary>
    /// If the event was for a card payment this is the name of the card processor,
    /// e.g. CyberSource or Checkout, that was used.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessorName { get; set; }

    /// <summary>
    /// Gets the amount to display with the correct number of decimal places based on the currency type. 
    /// </summary>
    /// <returns>The decimal amount to display for the payment request's currency.</returns>
    public decimal DisplayAmount()
    {
        return EventType switch
        {
            PaymentRequestEventTypesEnum.lightning_invoice_created => Math.Round(Amount, PaymentsConstants.BITCOIN_LIGHTNING_ROUNDING_DECIMAL_PLACES),
            _ => Math.Round(Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES)
        };
    }
}
