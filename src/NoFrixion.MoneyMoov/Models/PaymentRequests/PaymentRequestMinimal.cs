// -----------------------------------------------------------------------------
//  Filename: PaymentRequestMinimal.cs
// 
//  Description: Contains a minimal number of fields for a payment request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  25 11 2022  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models.PaymentRequests;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestMinimal
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    public string? MerchantName { get; set; }

    public string? MerchantShortName { get; set; }
    
    public string? MerchantLogoUrlPng { get; set; }
    
    public string? MerchantLogoUrlSvg { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency of the request.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// The title of the payment request.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// An optional description for the payment request. If set this field will appear
    /// on the transaction record for some card processors.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The card processor
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; }

    /// <summary>
    /// The card processors public key 
    /// </summary>
    public string? PaymentProcessorKey { get; set; }

    public string? CallbackUrl { get; set; }

    public string? CardStripePaymentIntentSecret { get; set; }

    /// <summary>
    /// The jwk containing the public key
    /// </summary>
    public string? Jwk { get; set; }

    /// <summary>
    /// The payment methods that the payment request supports. When setting using form data
    /// should be supplied as a comma separated list, for example "card, pisp, lightning, applePay".
    /// </summary>
    [Obsolete("This field has been deprecated. Please use PaymentMethodsList instead.")]
    public PaymentMethodTypeEnum PaymentMethods
    {
        get =>
            PaymentMethodsList.Aggregate(PaymentMethodTypeEnum.None, (current, method) => current | method);
    }

    /// <summary>
    /// The payment methods that the payment request supports.
    /// </summary>
    public List<PaymentMethodTypeEnum> PaymentMethodsList { get; set; } = new List<PaymentMethodTypeEnum>();

    /// <summary>
    /// This is the error returned from the bank which is recorded in payment request events.
    /// </summary>
    public string PispError { get; set; } = string.Empty;

    public Guid? PriorityBankID { get; set; }

    /// <summary>
    /// Merchant ID from Google Pay
    /// </summary>
    public string? GooglePayMerchantID { get; set; }

    /// <summary>
    /// The payment attempts for this payment request.
    /// </summary>
    public IEnumerable<PaymentRequestPaymentAttempt>? PaymentAttempts { get; set; }

    /// <summary>
    /// The status of the payment request.
    /// </summary>
    public PaymentResultEnum Status { get; set; }

    public PartialPaymentMethodsEnum PartialPaymentMethod { get; set; }

    /// <summary>
    /// Account ID of connected customers in Stripe
    /// </summary>
    public string? StripeAccountID { get; set; }

    /// <summary>
    /// The country code associated with the payment.
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// Lightning invoice ID, if any.
    /// </summary>
    [Obsolete("This field is deprecated. Lightning payments are no longer supported.")]
    public string? LightningInvoice { get; set; }

    /// <summary>
    /// Date and time of expiration of the lightning invoice.
    /// </summary>
    [Obsolete("This field is deprecated. Lightning payments are no longer supported.")]
    public DateTimeOffset? LightningInvoiceExpiresAt { get; set; }
    
    /// <summary>
    /// Custom fields to display to the customer.
    /// </summary>
    public List<PaymentRequestCustomField> CustomFieldsToDisplay { get; set; } = [];
}
