//-----------------------------------------------------------------------------
// Filename: PaymentRequestCreate.cs
//
// Description: The model used for creating new payment requests.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 23 Mar 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NoFrixion.MoneyMoov.Attributes;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestCreate : IValidatableObject, IPaymentRequest
{
    /// <summary>
    /// THe maximum number of tags that can be set when creating a payment request.
    /// </summary>
    public const int MAXIMUM_TAG_COUNT = 10;

    /// <summary>
    /// The ID of the merchant to create the payment request for.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// The amount of money to request.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }

    /// <summary>
    /// The currency of the payment request.
    /// </summary>
    [EnumDataType(typeof(CurrencyTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// An optional customer identifier for the payment request. This field is sent to the 
    /// payer's bank when using payment initiation. The restriction in the available characters
    /// is due to some banks rejecting requests when ones outside the set are used.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.CUSTOMER_ID_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.CUSTOMER_ID_ERROR_MESSAGE)]
    public string? CustomerID { get; set; }

    /// <summary>
    /// An optional order ID for the payment request. If the request is for an invoice this
    /// is the most appropriate field for the invoice ID.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.ORDER_ID_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.ORDER_ID_ERROR_MESSAGE)]
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
    /// Optionally the first name of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_FIRST_NAME_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_FIRST_NAME_ERROR_MESSAGE)]
    public string? ShippingFirstName { get; set; }

    /// <summary>
    /// Optionally the last name of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_LAST_NAME_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_LAST_NAME_ERROR_MESSAGE)]
    public string? ShippingLastName { get; set; }

    /// <summary>
    /// Optionally the first line of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_ADDRESS_LINE_1_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_ADDRESS_LINE_1_ERROR_MESSAGE)]
    public string? ShippingAddressLine1 { get; set; }

    /// <summary>
    /// Optionally the second line of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_ADDRESS_LINE_2_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_ADDRESS_LINE_2_ERROR_MESSAGE)]
    public string? ShippingAddressLine2 { get; set; }

    /// <summary>
    /// Optionally the city of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_CITY_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_CITY_ERROR_MESSAGE)]
    public string? ShippingAddressCity { get; set; }

    /// <summary>
    /// Optionally the state or county of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_COUNTY_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_COUNTY_ERROR_MESSAGE)]
    public string? ShippingAddressCounty { get; set; }

    /// <summary>
    /// Optionally the post code of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_POSTAL_CODE_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_POSTAL_CODE_ERROR_MESSAGE)]
    public string? ShippingAddressPostCode { get; set; }

    /// <summary>
    /// Optionally the country code of the customer's shipping address.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_COUNTRY_CODE_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_COUNTRY_CODE_ERROR_MESSAGE)]
    public string? ShippingAddressCountryCode { get; set; }

    /// <summary>
    /// Optionally the shipping phone number for the customer.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.SHIPPING_PHONE_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.SHIPPING_PHONE_ERROR_MESSAGE)]
    public string? ShippingPhone { get; set; }

    /// <summary>
    /// Optionally the shipping email address for the customer.
    /// </summary>
    [EmailAddress]
    public string? ShippingEmail { get; set; }

    [Obsolete("Please use BaseOriginUrl.")]
    public string? OriginUrl 
    { 
        get => BaseOriginUrl; 
        set => BaseOriginUrl = value; 
    }

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
    /// <remarks>
    /// The CardCreateToken and CardCreateTokenMode operate in the following way:
    ///  1. CardCreateToken is true and CardCreateTokenMode is None, then tokenisation WILL always
    ///  be attempted and the pay element WILL NOT display a disclaimer.
    ///  2. CardCreateTokenMode is "ConsentNotRequired", then tokenisation WILL always be attempted and 
    ///  the pay element WILL display a disclaimer with no option. The CardCreateToken will always be
    ///  overridden and set to true if this consent option is used. It does not make sense to display the 
    ///  disclaimer to the payer and then not attempt tokenisation.
    ///  3. If CardCreateTokenMode is "UserConsentRequired", then tokenisation  WILL ONLY be attempted 
    ///  if the user opts in. The CardCreateToken will be set to false when the payment request is created
    ///  and it's up to the pay element, or other client, to update the flag based on the user consent choice.
    /// </remarks>
    public bool CardCreateToken { get; set; }

    /// <summary>
    /// This specifies whether user consent will be taken before tokenising card or not.
    /// This cannot be 'None' if CardCreateToken is set to true. If this is set to 'UserConsentRequired'
    /// then, the user consent will overwrite CardCreateToken flag on submit card payment.
    /// </summary>
    public CardTokenCreateModes CardCreateTokenMode { get; set; }

    /// <summary>
    /// If set to true for card payments the sensitive card number and card verification number 
    /// will be transmitted directly rather than being tokenised. This makes the payment quicker
    /// but more exposed to client side flaws such as cross site scripting.
    /// </summary>
    public bool CardTransmitRawDetails { get; set; }

    /// <summary>
    /// Optional field that if specified indicates the processor merchant ID that should be used
    /// to process any card payments. Mainly useful where a merchant has multiple processor
    /// merchant ID's. If left empty the default merchant card settings will be used.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.CARD_PROCESSOR_MERCHANT_ID_CHARS_REGEX,
    ErrorMessage = PaymentRequestConstants.CARD_PROCESSOR_MERCHANT_ID_ERROR_MESSAGE)]
    public string? CardProcessorMerchantID { get; set; }

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
    /// For Payment Initiation payments this is the reference that will be requested to used as the reference 
    /// on the your transaction record. Note that it is not guaranteed that the sending bank will use this
    /// reference and in practice it has been observed to be supported by only half to two thirds of banks.
    /// </summary>
    //[RegularExpression(@"^([a-zA-Z0-9 ]{6,18})$",
    //ErrorMessage = @"The recipient reference must be between 6 and 18 characters and can only contain alphanumeric characters and space.")]
    [Obsolete("This field will be set automatically to ensure the best chance of matching a payin to a payment reuqest.")]
    public string? PispRecipientReference { get; set; }

    /// <summary>
    /// If set to true, and the merchant is configured for hosted payment pages, the base and callback URLs
    /// will be set to use the hosted payment page.
    /// </summary>
    public bool UseHostedPaymentPage { get; set; }

    /// <summary>
    /// If set to true for card payments no attempt will be made to use payer authentication (3-D Secure and equivalent).
    /// Skipping payer authentication can help avoid failed payment attempts when a payer is not enrolled or when they
    /// can't be bothered completing their issuing bank's authentication steps. A disadvantage is it exposes the merchant 
    /// to liability for charge backs.
    /// </summary>
    public bool CardNoPayerAuthentication { get; set; }

    /// <summary>
    /// The approach to use, or not, for accepting partial payments.
    /// </summary>
    public PartialPaymentMethodsEnum PartialPaymentMethod { get; set; }

    /// <summary>
    /// Optional email address for the customer. If the tokenise card option is set then the customer email address
    /// is mandatory.
    /// </summary>
    [EmailAddress]
    public string? CustomerEmailAddress { get; set; }

    [JsonIgnore]
    public PaymentProcessorsEnum PaymentProcessor { get; set; }

    [JsonIgnore]
    public string? LightningInvoice { get; set; }
    
    [EmailAddressMultiple(ErrorMessage = PaymentRequestConstants.NOTIFICATION_EMAIL_ADDRESSES_ERROR_MESSAGE)]
    public string? NotificationEmailAddresses { get; set; }

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
    /// An optional list of tag ids to add to the payment request
    /// </summary>
    public List<Guid>? TagIds { get; set; }

    /// <summary>
    /// An optional list of tag values to set on the payment request. If no matching
    /// tag exists it will be created.
    /// </summary>
    public List<string>? Tags { get; set; }
    
    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult>? validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        if (isValid)
        {
            // If the property validations passed, apply the overall business validation rules.
            var bizRulesvalidationResults = PaymentRequestValidator.Validate(this, context);
            if (bizRulesvalidationResults.Count() > 0)
            {
                isValid = false;
                validationResults ??= new List<ValidationResult>();
                validationResults.AddRange(bizRulesvalidationResults);
            }
        }

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem("The Payment Request create model had one or more validation errors.", validationResults);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return PaymentRequestValidator.Validate(this, validationContext);
    }

    /// <summary>
    /// Places all the payment request's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payment request's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(Amount), Amount.ToString());
        dict.Add(nameof(Currency), Currency.ToString());
        dict.Add(nameof(CustomerID), CustomerID ?? string.Empty);
        dict.Add(nameof(OrderID), OrderID ?? string.Empty);
        dict.Add(nameof(PaymentMethodTypes), PaymentMethodTypes.ToString());
        dict.Add(nameof(Description), Description ?? string.Empty);
        dict.Add(nameof(BaseOriginUrl), BaseOriginUrl!);
        dict.Add(nameof(CallbackUrl), CallbackUrl!);
        dict.Add(nameof(FailureCallbackUrl), FailureCallbackUrl ?? string.Empty);
        dict.Add(nameof(CardAuthorizeOnly), CardAuthorizeOnly.ToString());
        dict.Add(nameof(CardCreateToken), CardCreateToken.ToString());
        dict.Add(nameof(CardTransmitRawDetails), CardTransmitRawDetails.ToString());
        dict.Add(nameof(IgnoreAddressVerification), IgnoreAddressVerification.ToString());
        dict.Add(nameof(CardIgnoreCVN), CardIgnoreCVN.ToString());
        dict.Add(nameof(CardNoPayerAuthentication), CardNoPayerAuthentication.ToString());
        dict.Add(nameof(PispAccountID), PispAccountID?.ToString() ?? string.Empty);
        dict.Add(nameof(ShippingFirstName), ShippingFirstName ?? string.Empty);
        dict.Add(nameof(ShippingLastName), ShippingLastName ?? string.Empty);
        dict.Add(nameof(ShippingAddressLine1), ShippingAddressLine1 ?? string.Empty);
        dict.Add(nameof(ShippingAddressLine2), ShippingAddressLine2 ?? string.Empty);
        dict.Add(nameof(ShippingAddressCity), ShippingAddressCity ?? string.Empty);
        dict.Add(nameof(ShippingAddressCounty), ShippingAddressCounty ?? string.Empty);
        dict.Add(nameof(ShippingAddressPostCode), ShippingAddressPostCode ?? string.Empty);
        dict.Add(nameof(ShippingAddressCountryCode), ShippingAddressCountryCode ?? string.Empty);
        dict.Add(nameof(ShippingPhone), ShippingPhone ?? string.Empty);
        dict.Add(nameof(ShippingEmail), ShippingEmail ?? string.Empty);
        dict.Add(nameof(CustomerEmailAddress), CustomerEmailAddress ?? string.Empty);
        dict.Add(nameof(CardProcessorMerchantID), CardProcessorMerchantID ?? string.Empty);
        dict.Add(nameof(PartialPaymentMethod), PartialPaymentMethod.ToString());
        dict.Add(nameof(UseHostedPaymentPage), UseHostedPaymentPage.ToString());
        dict.Add(nameof(SuccessWebHookUrl), SuccessWebHookUrl ?? string.Empty);
        dict.Add(nameof(Title), Title ?? string.Empty);
        dict.Add(nameof(PartialPaymentSteps), PartialPaymentSteps ?? string.Empty);
        dict.Add(nameof(NotificationEmailAddresses), NotificationEmailAddresses ?? string.Empty);

        if (TagIds?.Count() > 0)
        {
            int tagIdNumber = 0;
            foreach (var tagId in TagIds)
            {
                dict.Add($"{nameof(TagIds)}[{tagIdNumber}]", tagId.ToString());
                tagIdNumber++;
            }
        }

        if (Tags?.Count() > 0)
        {
            int tagNumber = 0;
            foreach(var tag in Tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    dict.Add($"{nameof(Tags)}[{tagNumber}]", tag.Trim());

                    tagNumber++;
                    if (tagNumber > MAXIMUM_TAG_COUNT)
                    {
                        break;
                    }
                }
            }
        }

        return dict;
    }
}
