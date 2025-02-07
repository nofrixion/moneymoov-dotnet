// -----------------------------------------------------------------------------
//  Filename: PaymentRequestCsvExportModel.cs
// 
//  Description: Flat model for exporting payment requests to CSV.
//
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  06 Feb 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Attributes;

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestCsvExportModel
{
    public required Guid ID { get; set; }

    public required Guid MerchantID { get; set; }
    
    public string? MerchantName { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    [CsvIgnore]
    public required decimal Amount { get; set; }
    
    /// <summary>
    /// Gets the amount to display with the correct number of decimal places based on the currency type. 
    /// </summary>
    /// <returns>The decimal amount to display for the payment request's currency.</returns>
    [CsvColumn("Amount")]
    public decimal DisplayAmount => PaymentAmount.GetRoundedAmount(Currency, Amount);

    /// <summary>
    /// The currency of the request.
    /// </summary>
    public required CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// An optional customer identifier for the payment request.
    /// </summary>
    public string? CustomerID { get; set; }

    /// <summary>
    /// An optional order ID for the payment request. If the request is for an invoice this
    /// is the most appropriate field for the invoice ID.
    /// </summary>
    public string? OrderID { get; set; }
    
    public required List<PaymentMethodTypeEnum> PaymentMethods { get; set; }

    /// <summary>
    /// An optional description for the payment request. If set this field will appear
    /// on the transaction record for some card processors.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The payment account ID to use to receive payment initiation payments. This must match one of your
    /// NoFrixion payment account IDs. This can be left blank to use your default payment account.
    /// </summary>
    public Guid? PayByBankDestinationAccountID { get; set; }
    
    public string? PayByBankDestinationAccountName { get; set; }

    /// <summary>
    /// For card payments the default behaviour is to authorise and capture the payment at the same
    /// time. If a merchant needs to authorise and then capture at a later point this property needs
    /// to be set to true.
    /// </summary>
    public bool CardAuthorizeOnly { get; set; }

    /// <summary>
    /// For card payments a payment attempt can be used to create a reusable token for subsequent
    /// payments. Setting this field to true will create a reusable customer token.
    /// </summary>
    public bool CardCreateToken { get; set; }

    /// <summary>
    /// This specifies whether user consent will be taken before tokenising card or not.
    /// If this is set to 'UserConsentRequired' then, the user consent will potentially update 
    /// CardCreateToken flag on submit card payment.
    /// </summary>
    public CardTokenCreateModes CardCreateTokenMode { get; set; }

    /// <summary>
    /// If the card payment option is enabled this field indicates which card processor
    /// the merchant is set up to use.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.CyberSource;

    /// <summary>
    /// The current status of the payment request. Will be set to FullyPaid when the full
    /// amount has been received.
    /// </summary>
    public PaymentResultEnum Status { get; set; }

    /// <summary>
    /// This is a convenience link generated for payment requests whose merchants are using
    /// hosted payment pages. The link will load the MoneyMoov hosted payment page. If the 
    /// merchant has not been set up for hosted payment or the payment can't be hosted then
    /// this property will be empty.
    /// </summary>
    public string? HostedPayCheckoutUrl { get; set; }

    /// <summary>
    /// The approach to use, or not, for accepting partial payments.
    /// </summary>
    public PartialPaymentMethodsEnum PartialPaymentMethod { get; set; }

    /// <summary>
    /// The timestamp the payment request was created at.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// The timestamp the payment request was last updated at.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// If set to true, and the merchant is configured for hosted payment pages, the base and callback URLs
    /// will be set to use the hosted payment page.
    /// </summary>
    public bool UseHostedPaymentPage { get; set; }

    /// <summary>
    /// Optional email address for the customer. If the tokenise card option is set then the customer email address
    /// is mandatory.
    /// </summary>
    public string? CustomerEmailAddress { get; set; }

    /// <summary>
    /// If Stripe is being used as the card payment processor this property is used to hold the Stripe payment intent ID.
    /// </summary>
    public string? CardStripePaymentIntentID { get; set; }

    /// <summary>
    /// If Stripe is being used as the card payment processor this property is used to hold the Stripe payment intent client secret.
    /// </summary>
    public string? CardStripePaymentIntentSecret { get; set; }

    public List<Guid>? TransactionIDs { get; set; }

    /// <summary>
    /// An optional list of descriptive tags attached to the payment request.
    /// </summary>
    public List<string> Tags { get; set; } = [];

    /// <summary>
    /// The ID of the bank that is set as the priority bank for display on pay element.
    /// </summary>
    public string? PriorityBankName { get; set; }

    /// <summary>
    /// A generic field to contain any additional data that the merchant wishes to store against the payment request.
    /// E.g. product or service information.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// The ID of a payrun that needs an account top up. 
    /// Payment request can be used to top up payrun accounts.  
    /// </summary>
    public Guid? PayrunID { get; set; }
    
    [CsvIgnore]
    public List<PaymentRequestAddress>? Addresses { get; set; } = []; 

    /// <summary>
    /// Attempts to get the billing address for this payment request.
    /// </summary>
    /// <returns>The billing address or null if it's not set.</returns>
    [CsvIgnore]
    public PaymentRequestAddress? BillingAddress =>
        Addresses?.Where(x => x.AddressType == AddressTypesEnum.Billing).FirstOrDefault();
    
    [CsvColumn("BillingAddress")]
    public string DisplayBillingAddress =>
        BillingAddress != null ? BillingAddress.ToDisplayString() : string.Empty;

    /// <summary>
    /// Attempts to get the shipping address for this payment request.
    /// </summary>
    /// <returns>The shipping address or null if it's not set.</returns>
    [CsvIgnore]
    public PaymentRequestAddress? ShippingAddress =>
        Addresses?.Where(x => x.AddressType == AddressTypesEnum.Shipping).FirstOrDefault();

    [CsvColumn("ShippingAddress")]
    public string DisplayShippingAddress =>
        ShippingAddress != null ? ShippingAddress.ToDisplayString() : string.Empty;
    

    public string? NotificationEmailAddresses { get; set; }
    
    /// <summary>
    /// Total amount received for this payment request.
    /// </summary>
    /// <returns>The total amount received in decimal.</returns>
    [CsvIgnore]
    public decimal AmountReceived { get; set; }
    
    [CsvColumn("AmountReceived")]
    public decimal DisplayAmountReceived => PaymentAmount.GetRoundedAmount(Currency, AmountReceived);
    
    /// <summary>
    /// Total amount refunded for this payment request.
    /// </summary>
    /// <returns>The total amount refunded in decimal.</returns>
    [CsvIgnore]
    public decimal AmountRefunded { get; set; }
    
    [CsvColumn("AmountRefunded")]
    public decimal DisplayAmountRefunded => PaymentAmount.GetRoundedAmount(Currency, AmountRefunded);
    
    /// <summary>
    /// Total amount that has been authorised but not settled for this payment request.
    /// </summary>
    /// <returns>The total amount pending in decimal.</returns>
    [CsvIgnore]
    public decimal AmountPending { get; set; }
    
    [CsvColumn("AmountPending")]
    public decimal DisplayAmountPending => PaymentAmount.GetRoundedAmount(Currency, AmountPending);
    
    /// <summary>
    /// Details of the user who created the Payment Request
    /// </summary>
    public string? CreatedByUserName { get; set; }
    
    /// <summary>
    /// Description of the merchant token in case the Payment request was created using a merchant token.
    /// </summary>
    public string? MerchantTokenDescription { get; set; }

    public string CustomerName =>
        Addresses != null && Addresses.Count != 0 ? $"{Addresses.First().FirstName} {Addresses.First().LastName}" : string.Empty;
    
    public string? PispRecipientReference { get; set; }
}