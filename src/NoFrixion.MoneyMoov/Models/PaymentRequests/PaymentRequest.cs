//-----------------------------------------------------------------------------
// Filename: PaymentRequest.cs
//
// Description: Represents an order placed by a customer to an e-commerce merchant.
// Originally used for the CyberSoure card gateway.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 01 Oct 2021  Aaron Clauson       Created, Carmichael House, Dublin, Ireland.
// 01 Dec 2021  Donal O'Connor      Updated.
// 06 Dec 2021  Áthila Naniny Ninow Added MerchantCategoryCode.
// 08 Dec 2021  Áthila Naniny Ninow Added AccountID.
// 13 Dec 2021  Aaron Clauson       Renamed from PaymentOrder to PaymentRequest.
// 14 Dec 2021  Aaron Clauson       Removed AmountReceived and PaymentMethod (using PaymentReceived instead).
// 21 Dec 2021  Aaron Clauson       Added OriginUrl and CallbackUrl.
// 23 Mar 2022  Aaron Clauson       Moved validation logic to PaymentRequestCreate. 
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Extensions;
using LanguageExt;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequest : IPaymentRequest, IWebhookPayload, IExportableToCsv
{
    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency of the request.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// An optional customer identifier for the payment request.
    /// </summary>
    public string? CustomerID { get; set; }

    /// <summary>
    /// An optional order ID for the payment request. If the request is for an invoice this
    /// is the most appropriate field for the invoice ID.
    /// </summary>
    public string? OrderID { get; set; }

    /// <summary>
    /// The payment methods that the payment request supports. When setting using form data
    /// should be supplied as a comma separated list, for example "card, pisp, lightning".
    /// </summary>
    [Obsolete("This field has been deprecated. Please use PaymentMethods instead.")]
    public PaymentMethodTypeEnum PaymentMethodTypes 
    { 
        get =>
           PaymentMethods.Any() ? PaymentMethods.ToFlagEnum() : PaymentMethodTypeEnum.None;
    }

    /// <summary>
    /// The payment methods that the payment request supports.
    /// </summary>
    public List<PaymentMethodTypeEnum> PaymentMethods { get; set; } = new List<PaymentMethodTypeEnum>();

    /// <summary>
    /// An optional description for the payment request. If set this field will appear
    /// on the transaction record for some card processors.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The payment account ID to use to receive payment initiation payments. This must match one of your
    /// NoFrixion payment account IDs. This can be left blank to use your default payment account.
    /// </summary>
    public Guid? PispAccountID { get; set; }

    /// <summary>
    /// For card payments the origin of the payment page needs to be set in advance.
    /// A public key context is generated to encrypt sensitive card details and is bound
    /// to a single origin URL.
    /// </summary>
    public string? BaseOriginUrl { get; set; }

    /// <summary>
    /// Once a payment is processed, or a notification of an inbound payment is received,
    /// a callback request will be made to this URL. Typically it will be the page on
    /// a merchant's web site that displays the results of the payment attempt.
    /// </summary>
    public string? CallbackUrl { get; set; }

    /// <summary>
    /// Optional callback URL for payment failures that can occur when the payer is 
    /// redirected away from the payment page. Typically the payer is only sent away
    /// from the payment page for pay by bank attempts. If this URL is not set the 
    /// payer will be redirected back to the original URL the payment attempt was initiated
    /// from.
    /// </summary>
    public string? FailureCallbackUrl { get; set; }

    /// <summary>
    /// If a payment event results in the payment request being classified as fully paid this
    /// success webhook URL will be invoked. The URL will be invoked as a GET request, i.e.
    /// there will be no request body. Two query parameters will be added to the URL. The 
    /// first one will be "id" and will hold the payment request ID. The second one will be
    /// "orderid" and will hold the payment request OrderID, note the OrderID could be empty
    /// if it was not set when the payment request was created.
    /// The recommended approach when receiving a success web hook is to use the "id" parameter
    /// to call the moneymoov get payment request endpoint to retrieve the full details of the
    /// payment request and check the status. Web hooks can be easily spoofed and should not be
    /// relied upon.
    /// </summary>
    public string? SuccessWebHookUrl { get; set; }

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
    /// If set to true the card payment gateway will be directed to proceed with a payment even if the
    /// address verification checks fails.
    /// </summary>
    public bool IgnoreAddressVerification { get; set; }

    /// <summary>
    /// If set to true the card payment gateway will be directed to proceed with a payment even if the
    /// card verification number check fails.
    /// </summary>
    public bool CardIgnoreCVN { get; set; }

    /// <summary>
    /// Optional field that if specified indicates the processor merchant ID that should be used
    /// to process any card payments. Mainly useful where a merchant has multiple processor
    /// merchant ID's. If left empty the default merchant card settings will be used.
    /// </summary>
    public string? CardProcessorMerchantID { get; set; }

    /// <summary>
    /// If the card payment option is enabled this field indicates which card processor
    /// the merchant is set up to use.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.CyberSource;

    /// <summary>
    /// For Payment Initiation payments this is the reference that will be requested to used as the reference 
    /// on the payee's transaction record. Note that it is not guaranteed that the sending bank will use this
    /// reference and in practice it has been observed to be supported by only half to two thirds of banks.
    /// </summary>
    [Obsolete("This field has been deprecated. Recipient reference will be set automatically to reconcile payments.")]
    public string? PispRecipientReference { get; set; }

    /// <summary>
    /// Bitcoin Lightning invoice for the payment request.
    /// </summary>
    public string? LightningInvoice { get; set; }

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
    /// The Inserted timestamp output as a sortable string 
    /// https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings#UniversalSortable 
    /// Format also supported natively by Javascript https://tc39.es/ecma262/#sec-date-time-string-format.
    /// </summary>
    public string? InsertedSortable { get; set; }

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
    [EmailAddress]
    public string? CustomerEmailAddress { get; set; }

    /// <summary>
    /// If Stripe is being used as the card payment processor this property is used to hold the Stripe payment intent ID.
    /// </summary>
    public string? CardStripePaymentIntentID { get; set; }

    /// <summary>
    /// If Stripe is being used as the card payment processor this property is used to hold the Stripe payment intent client secret.
    /// </summary>
    public string? CardStripePaymentIntentSecret { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Merchant? Merchant { get; set; }

    public List<PaymentRequestAddress> Addresses { get; set; } = new List<PaymentRequestAddress>();

    public List<PaymentRequestEvent> Events { get; set; } = new List<PaymentRequestEvent>();

    public List<CardCustomerToken> TokenisedCards { get; set; } = new List<CardCustomerToken>();

    public List<Transaction> Transactions { get; set; } = new List<Transaction>();

    /// <summary>
    /// An optional list of descriptive tags attached to the payment request.
    /// </summary>
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public PaymentRequestResult Result { get; set; } = new PaymentRequestResult { Result = PaymentResultEnum.None };

    /// <summary>
    /// The jwk containing the public key used to verify the signature of the payment request.
    /// </summary>
    public string? Jwk { get; set; }

    /// <summary>
    /// The ID of the bank that is set as the priority bank for display on pay element.
    /// </summary>
    public Guid? PriorityBankID { get; set; }

    /// <summary>
    /// A generic field to contain any additional data that the merchant wishes to store against the payment request.
    /// E.g. product or service information.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// An optional comma separated list of partial payment amounts. The amounts represent guidance, or suggestions, as to
    /// how the payer will be requested to make partial payments.
    /// </summary>
    public string? PartialPaymentSteps { get; set; }

    /// <summary>
    /// The ID of a payrun that needs an account top up. 
    /// Payment request can be used to top up payrun accounts.  
    /// </summary>
    public Guid? PayrunID { get; set; }

    /// <summary>
    /// Attempts to get the billing address for this payment request.
    /// </summary>
    /// <returns>The billing address or null if it's not set.</returns>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public PaymentRequestAddress? BillingAddress =>
        Addresses?.Where(x => x.AddressType == AddressTypesEnum.Billing).FirstOrDefault();

    /// <summary>
    /// Attempts to get the shipping address for this payment request.
    /// </summary>
    /// <returns>The shipping address or null if it's not set.</returns>
    public PaymentRequestAddress? ShippingAddress =>
        Addresses?.Where(x => x.AddressType == AddressTypesEnum.Shipping).FirstOrDefault();

    /// <summary>
    /// Gets the symbol for the payment request currency.
    /// </summary>
    public string DisplayCurrencySymbol() => Currency.GetCurrencySymbol();

    /// <summary>
    /// Gets the amount to display with the correct number of decimal places based on the currency type. 
    /// </summary>
    /// <returns>The decimal amount to display for the payment request's currency.</returns>
    public decimal DisplayAmount() => PaymentAmount.GetRoundedAmount(Currency, Amount);

    public string? NotificationEmailAddresses { get; set; }

    /// <summary>
    /// The payment attempts made against this payment request.
    /// </summary>
    public List<PaymentRequestPaymentAttempt> PaymentAttempts => this.Events.GetPaymentAttempts();
    
    /// <summary>
    /// Total amount received for this payment request.
    /// </summary>
    /// <returns>The total amount received in decimal.</returns>
    public decimal AmountReceived { get; set; }
    
    /// <summary>
    /// Total amount refunded for this payment request.
    /// </summary>
    /// <returns>The total amount refunded in decimal.</returns>
    public decimal AmountRefunded { get; set; }
    
    /// <summary>
    /// Total amount that has been authorised but not settled for this payment request.
    /// </summary>
    /// <returns>The total amount pending in decimal.</returns>
    public decimal AmountPending { get; set; }
    
    /// <summary>
    /// Details of the user who created the Payment Request
    /// </summary>
    public User? CreatedByUser { get; set; }
    
    /// <summary>
    /// Description of the merchant token in case the Payment request was created using a merchant token.
    /// </summary>
    public string? MerchantTokenDescription { get; set; }

    public string FormattedAmount => PaymentAmount.DisplayCurrencyAndAmount(Currency, Amount);

    /// <summary>
    /// Date and time of expiration of the lightning invoice.
    /// </summary>
    public DateTimeOffset? LightningInvoiceExpiresAt { get; set; }
    
    /// <summary>
    /// Minimal destination account details, if available.
    /// </summary>
    public PaymentAccount? DestinationAccount { get; set; }
    
    /// <summary>
    /// Sandbox only. Optional. If set, simulated settlements will be delayed by the specified number of seconds.
    /// </summary>
    public int? SandboxSettleDelayInSeconds { get; set; }

    public string CustomerName =>
        Addresses.Any() ? $"{Addresses.First().FirstName} {Addresses.First().LastName}" : string.Empty;

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(this, context);

        bool isValid = validationResults.Count() == 0;

        return isValid
                   ? NoFrixionProblem.Empty
                   : new NoFrixionProblem("The Payment Request had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Indicates whether the payment request has any of the HTTP web hook fields set.
    /// </summary>
    /// <returns>True if this payment request has any of the web hook callback fields set, false otherwise.</returns>
    public bool HasWebHook()
    {
        return !string.IsNullOrEmpty(SuccessWebHookUrl);
    }

    public Either<NoFrixionProblem, Uri> GetSuccessWebhookUri()
    {
        if (string.IsNullOrWhiteSpace(SuccessWebHookUrl))
        {
            return new Uri(MoneyMoovConstants.WEBHOOK_BLACKHOLE_URI);
        }
        else
        {
            if (Uri.TryCreate(SuccessWebHookUrl, UriKind.Absolute, out Uri? uri))
            {
                var successWebHookUri = new UriBuilder(uri);

                string successParams = $"id={ID}&orderid={OrderID ?? string.Empty}";

                successWebHookUri.Query = string.IsNullOrEmpty(successWebHookUri.Query)
                                              ? successParams
                                              : successWebHookUri.Query + "&" + successParams;

                return successWebHookUri.Uri;
            }
            else
            {
                return new NoFrixionProblem($"The success web hook URL {SuccessWebHookUrl} for payment request ID {ID} is not a valid URL.");
            }
        }
    }

    /// <summary>
    /// Gets the total amount received for this payment request.
    /// </summary>
    /// <returns>The total amount received in decimal.</returns>
    public decimal GetTotalAmountReceived()
    {
        if (!PaymentAttempts.Any())
        {
            return 0;
        }

        return PaymentAttempts
            .Sum(pa =>
                pa.PaymentMethod switch
                {
                    PaymentMethodTypeEnum.card => pa.CardAuthorisedAmount,
                    PaymentMethodTypeEnum.pisp => pa.SettledAmount,
                    PaymentMethodTypeEnum.lightning => pa.SettledAmount,
                    PaymentMethodTypeEnum.directDebit => pa.SettledAmount,
                    _ => 0
                });
    }

    /// <summary>
    /// Gets the total amount refunded for this payment request.
    /// </summary>
    /// <returns>The total amount refunded in decimal.</returns>
    public decimal GetTotalAmountRefunded()
    {
        if (!PaymentAttempts.Any())
        {
            return 0;
        }

        return PaymentAttempts
            .Sum(pa => pa.RefundAttempts
                .Sum(ra => ra.RefundSettledAmount));
    }

    /// <summary>
    /// Gets the total amount pending for this payment request.
    /// This is mainly for PISP payments where the amount is
    /// authorised but not yet settled.
    /// </summary>
    /// <returns>The total amount pending in decimal.</returns>
    public decimal GetTotalAmountPending()
    {
        if (!PaymentAttempts.Any())
        {
            return 0;
        }

        return PaymentAttempts
            .Sum(pa =>
                pa.PaymentMethod switch
                {
                    PaymentMethodTypeEnum.pisp => pa.AuthorisedAmount - pa.SettledAmount,
                    _ => 0
                });
    }

    public string CsvHeader() =>
        "ID,MerchantID,Amount,Currency,PaymentMethods,Description,CustomerID,OrderID,Inserted,LastUpdated,PispAccountID,PispAccountName,BaseOriginUrl,CardAuthorizeOnly,CardCreateToken,CardCreateTokenMode,Status,PartialPaymentMethod,CustomerEmailAddress,CardStripePaymentIntentID,CardStripePaymentIntentSecret,NotificationEmailAddresses,PriorityBankID,Title,PartialPaymentSteps,AmountReceived,AmountRefunded,AmountPending,CreatedByUserID,CreatedByUserName,MerchantTokenDescription,TransactionIDs,PayrunID,ShippingAddress,BillingAddress,CustomerName,PaymentProcessor,Tags";
    
    public string ToCsvRow()
    {
        return this.ToCsvRowString();
    }
}
