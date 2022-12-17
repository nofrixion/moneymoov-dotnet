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
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequest : IPaymentRequest
{
    /// <summary>
    /// For cases where the URI supplied doesn't need to be sent.
    /// </summary>
    public const string SUCCESS_WEBHOOK_BLACKHOLE_URI = "http://127.0.0.1";

    public Guid ID { get; set; }

    public Guid MerchantID { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency of the request.
    /// </summary>
    [EnumDataType(typeof(CurrencyTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
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
    [EnumDataType(typeof(PaymentMethodTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMethodTypeEnum PaymentMethodTypes { get; set; } = PaymentMethodTypeEnum.card;

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
    /// For Payment Initiation payments this is the reference that will appear on
    /// the recipients transaction record.
    /// </summary>
    public string? PispRecipientReference { get; set; }

    /// <summary>
    /// Bitcoin Lightning invoice for the payment request.
    /// </summary>
    public string? LightningInvoice {  get; set; }

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

    public Merchant? Merchant { get; set; }

    public List<PaymentRequestAddress> Addresses { get; set; } = new List<PaymentRequestAddress>();

    public List<PaymentRequestEvent> Events { get; set; } = new List<PaymentRequestEvent>();

    public List<CardCustomerToken> TokenisedCards { get; set; } = new List<CardCustomerToken>();

    public PaymentRequestResult Result { get; set; } = new PaymentRequestResult { Result = PaymentResultEnum.None };

    /// <summary>
    /// The jwk containing the public key used to verify the signature of the payment request.
    /// </summary>
    public string? Jwk { get; set; }
    
    /// <summary>
    /// Attempts to get the billing address for this payment request.
    /// </summary>
    /// <returns>The billing address or null if it's not set.</returns>
    [JsonIgnore]
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
    public string DisplayCurrencySymbol() => GetCurrencySymbol(Currency);

    /// <summary>
    /// Gets the amount to display with the correct number of decimal places based on the currency type. 
    /// </summary>
    /// <returns>The decimal amount to display for the payment request's currency.</returns>
    public decimal DisplayAmount() =>
        IsFiat(Currency) ? Math.Round(Amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES) : Amount;

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);
        var validationResults = PaymentRequestValidator.Validate(this, context);

        bool isValid = validationResults.Count() == 0;

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem("The Payment Request had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Indicates whether the payment request has any of the HTTP web hook fields set.
    /// </summary>
    /// <returns>True if this payment request has any of the web hook callback fields set, false otherwise.</returns>
    public bool HasWebHook()
    {
        return !string.IsNullOrEmpty(SuccessWebHookUrl);
    }

    public Uri GetSuccessWebhookUri()
    {
        if (string.IsNullOrEmpty(SuccessWebHookUrl))
        {
            return new Uri(SUCCESS_WEBHOOK_BLACKHOLE_URI);
        }
        else
        {
            var successWebHookUri = new UriBuilder(SuccessWebHookUrl);

            string successParams = $"id={ID}&orderid={OrderID ?? string.Empty}";

            successWebHookUri.Query = string.IsNullOrEmpty(successWebHookUri.Query)
                ? successParams
                : successWebHookUri.Query + "&" + successParams;

            return successWebHookUri.Uri;
        }
    }

    public static string GetCurrencySymbol(CurrencyTypeEnum currency) =>
     currency switch
     {
         CurrencyTypeEnum.BTC => "₿",
         CurrencyTypeEnum.LBTC => "⚡",
         CurrencyTypeEnum.GBP => "£",
         CurrencyTypeEnum.EUR => "€",
         _ => "€"
     };

    public static decimal GetDisplayAmount(decimal amount, CurrencyTypeEnum currency)
    {
        return IsFiat(currency) ? Math.Round(amount, PaymentsConstants.FIAT_ROUNDING_DECIMAL_PLACES) : amount;
    }

    /// <summary>
    /// Combines the display currency symbol and amount.
    /// </summary>
    public static string DisplayCurrencyAndAmount(CurrencyTypeEnum currency, decimal amount) =>
        GetCurrencySymbol(currency) + (IsFiat(currency) ? amount.ToString("0.00") : amount);

    private static bool IsFiat(CurrencyTypeEnum currency) =>
        currency switch
        {
            CurrencyTypeEnum.BTC or CurrencyTypeEnum.LBTC => false,
            _ => true
        };
}
