// -----------------------------------------------------------------------------
//  Filename: PaymentRequestUpdate.cs
// 
//  Description: Contains details of a payment request update request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  23 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Attributes;
using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Models.PaymentRequests;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestUpdate
{
    public decimal? Amount { get; set; }
    public CurrencyTypeEnum? Currency { get; set; }

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
    [Obsolete("This field has been deprecated. Please use PaymentMethods instead.")]
    public PaymentMethodTypeEnum? PaymentMethodTypes
    {
        get =>
            PaymentMethods != null && PaymentMethods.Any() ? PaymentMethods.ToFlagEnum() : PaymentMethodTypeEnum.None;

        init
        {
            if (value != null)
            {
                if (PaymentMethods == null)
                {
                    PaymentMethods = new();
                }

                if (value == PaymentMethodTypeEnum.None)
                {
                    PaymentMethods.Clear();
                }
                else
                {
                    PaymentMethods = value.Value.ToList();
                }
            }
        }
    }

    /// <summary>
    /// The payment methods that the payment request supports.
    /// </summary>
    public List<PaymentMethodTypeEnum>? PaymentMethods { get; set; }

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
    /// For card payments the default behaviour is to authorise and capture the payment at the same
    /// time. If a merchant needs to authorise and then capture at a later point this property needs
    /// to be set to true.
    /// </summary>
    public bool? CardAuthorizeOnly { get; set; }

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
    public bool? CardCreateToken { get; set; }

    /// <summary>
    /// This specifies whether user consent will be taken before tokenising card or not.
    /// This cannot be 'None' if CardCreateToken is set to true. If this is set to 'UserConsentRequired'
    /// then, the user consent will overwrite CardCreateToken flag on submit card payment.
    /// </summary>
    public CardTokenCreateModes? CardCreateTokenMode { get; set; }
    public bool? IgnoreAddressVerification { get; set; }
    public bool? CardIgnoreCVN { get; set; }

    [Obsolete("This field has been deprecated. Recipient reference will be set automatically to reconcile payments.")]
    public string? PispRecipientReference { get; set; }

    /// <summary>
    /// Optional field that if specified indicates the processor merchant ID that should be used
    /// to process any card payments. Mainly useful where a merchant has multiple processor
    /// merchant ID's. If left empty the default merchant card settings will be used.
    /// </summary>
    [RegularExpression(PaymentRequestConstants.CARD_PROCESSOR_MERCHANT_ID_CHARS_REGEX,
        ErrorMessage = PaymentRequestConstants.CARD_PROCESSOR_MERCHANT_ID_ERROR_MESSAGE)]
    public string? CardProcessorMerchantID { get; set; }

    /// <summary>
    /// Optional email address for the customer. If the tokenise card option is set then the customer email address
    /// is mandatory.
    /// </summary>
    [EmailAddress]
    public string? CustomerEmailAddress { get; set; }

    [EmailAddressMultiple(ErrorMessage = "One or more of the email addresses are invalid. Addresses can be separated by a comma, semi-colon or space.")]
    public string? NotificationEmailAddresses { get; set; }

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
    /// Bitcoin Lightning invoice for the payment request.
    /// </summary>
    public string? LightningInvoice { get; set; }

    /// <summary>
    /// Date and time of expiration of the lightning invoice.
    /// </summary>
    public DateTimeOffset? LightningInvoiceExpiresAt { get; set; }
    
    /// <summary>
    /// If set to true, a receipt will be automatically sent to the CustomerEmailAddress when payments are received.
    /// </summary>
    public bool? AutoSendReceipt { get; set; }
    
    /// <summary>
    /// A list of custom fields to add to the payment request. The custom fields
    /// are data type agnostic which means that the API will not do any validation or formatting
    /// in the key-value pairs. The API will store the custom fields as is.
    /// </summary>
    public List<PaymentRequestCustomFieldCreate>? CustomFields { get; set; }
    
    /// <summary>
    /// Places all the payment request's properties into a dictionary. Useful for testing
    /// when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the payment request's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (Amount != null) dict.Add(nameof(Amount), Amount.Value.ToString());
        if (Currency != null) dict.Add(nameof(Currency), Currency.Value.ToString());
        if (CustomerID != null) dict.Add(nameof(CustomerID), CustomerID);
        if (OrderID != null) dict.Add(nameof(OrderID), OrderID);
        if (Description != null) dict.Add(nameof(Description), Description);
        if (PispAccountID != null) dict.Add(nameof(PispAccountID), PispAccountID.GetValueOrDefault().ToString());
        if (ShippingFirstName != null) dict.Add(nameof(ShippingFirstName), ShippingFirstName);
        if (ShippingLastName != null) dict.Add(nameof(ShippingLastName), ShippingLastName);
        if (ShippingAddressLine1 != null) dict.Add(nameof(ShippingAddressLine1), ShippingAddressLine1);
        if (ShippingAddressLine2 != null) dict.Add(nameof(ShippingAddressLine2), ShippingAddressLine2);
        if (ShippingAddressCity != null) dict.Add(nameof(ShippingAddressCity), ShippingAddressCity);
        if (ShippingAddressCounty != null) dict.Add(nameof(ShippingAddressCounty), ShippingAddressCounty);
        if (ShippingAddressPostCode != null) dict.Add(nameof(ShippingAddressPostCode), ShippingAddressPostCode);
        if (ShippingAddressCountryCode != null) dict.Add(nameof(ShippingAddressCountryCode), ShippingAddressCountryCode);
        if (ShippingPhone != null) dict.Add(nameof(ShippingPhone), ShippingPhone);
        if (ShippingEmail != null)  dict.Add(nameof(ShippingEmail), ShippingEmail);
        if (BaseOriginUrl != null) dict.Add(nameof(BaseOriginUrl), BaseOriginUrl);
        if (CallbackUrl != null) dict.Add(nameof(CallbackUrl), CallbackUrl);
        if (FailureCallbackUrl != null) dict.Add(nameof(FailureCallbackUrl), FailureCallbackUrl);
        if (CardAuthorizeOnly != null) dict.Add(nameof(CardAuthorizeOnly), CardAuthorizeOnly.Value.ToString());
        if (CardCreateToken != null) dict.Add(nameof(CardCreateToken), CardCreateToken.Value.ToString());
        if (IgnoreAddressVerification != null) dict.Add(nameof(IgnoreAddressVerification), IgnoreAddressVerification.Value.ToString());
        if (CardIgnoreCVN != null) dict.Add(nameof(CardIgnoreCVN), CardIgnoreCVN.Value.ToString());
        if (CardProcessorMerchantID != null) dict.Add(nameof(CardProcessorMerchantID), CardProcessorMerchantID);
        if (CustomerEmailAddress != null) dict.Add(nameof(CustomerEmailAddress), CustomerEmailAddress ?? string.Empty);
        if (NotificationEmailAddresses != null) dict.Add(nameof(NotificationEmailAddresses), NotificationEmailAddresses ?? string.Empty);
        if (Title != null) dict.Add(nameof(Title), Title);
        if (PartialPaymentSteps != null) dict.Add(nameof(PartialPaymentSteps), PartialPaymentSteps);
        if(AutoSendReceipt != null) dict.Add(nameof(AutoSendReceipt), AutoSendReceipt.Value.ToString());
        if (CustomFields != null && CustomFields.Count > 0)
        {
            var customFieldNumber = 0;
            foreach (var customField in CustomFields.Where(x =>
                         !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.Value)))
            {
                dict.Add($"{nameof(CustomFields)}[{customFieldNumber}].{nameof(customField.Name)}", customField.Name!);
                dict.Add($"{nameof(CustomFields)}[{customFieldNumber}].{nameof(customField.Value)}",
                    customField.Value!);
                dict.Add($"{nameof(CustomFields)}[{customFieldNumber}].{nameof(customField.DisplayForPayer)}",
                    customField.DisplayForPayer.ToString());
                customFieldNumber++;
            }
        }

        if (PaymentMethods?.Count() > 0)
        {
            int paymentMethodNumber = 0;
            foreach (var paymentMethod in PaymentMethods)
            {
                dict.Add($"{nameof(PaymentMethods)}[{paymentMethodNumber}]", paymentMethod.ToString());
                paymentMethodNumber++;
            }
        }

        return dict;
    }
}