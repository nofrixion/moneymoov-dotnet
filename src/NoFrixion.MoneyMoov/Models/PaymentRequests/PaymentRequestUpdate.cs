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

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestUpdate
{
    public decimal? Amount { get; set; }
    public CurrencyTypeEnum? Currency { get; set; }
    public string? CustomerID { get; set; }
    public string? OrderID { get; set; }
    public PaymentMethodTypeEnum? PaymentMethodTypes { get; set; }
    public string? Description { get; set; }
    public string? PispAccountID { get; set; }
    public string? ShippingFirstName { get; set; }
    public string? ShippingLastName { get; set; }
    public string? ShippingAddressLine1 { get; set; }
    public string? ShippingAddressLine2 { get; set; }
    public string? ShippingAddressCity { get; set; }
    public string? ShippingAddressCounty { get; set; }
    public string? ShippingAddressPostCode { get; set; }
    public string? ShippingAddressCountryCode { get; set; }
    public string? ShippingPhone { get; set; }
    public string? ShippingEmail { get; set; }
    public string? BaseOriginUrl { get; set; }
    public string? CallbackUrl { get; set; }
    public bool? CardAuthorizeOnly { get; set; }
    public bool? CardCreateToken { get; set; }
    public bool? IgnoreAddressVerification { get; set; }
    public bool? CardIgnoreCVN { get; set; }
    public string? PispRecipientReference { get; set; }
    [RegularExpression(@"[a-zA-Z0-9]+",
        ErrorMessage = @"The CardProcessorMerchantID can only contain alphanumeric characters.")]
    public string? CardProcessorMerchantID { get; set; }
    [EmailAddress]
    public string? CustomerEmailAddress { get; set; }

    [EmailAddressMultiple(ErrorMessage = "One or more of the email addresses are invalid. Addresses can be separated by a comma, semi-colon or space.")]
    public string? NotificationEmailAddresses { get; set; }
    public string? Title { get; set; }
    public string? PartialPaymentSteps { get; set; }

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
        if (PaymentMethodTypes != null) dict.Add(nameof(PaymentMethodTypes), PaymentMethodTypes.Value.ToString());
        if (Description != null) dict.Add(nameof(Description), Description);
        if (PispAccountID != null) dict.Add(nameof(PispAccountID), PispAccountID);
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
        if (CardAuthorizeOnly != null) dict.Add(nameof(CardAuthorizeOnly), CardAuthorizeOnly.Value.ToString());
        if (CardCreateToken != null) dict.Add(nameof(CardCreateToken), CardCreateToken.Value.ToString());
        if (IgnoreAddressVerification != null) dict.Add(nameof(IgnoreAddressVerification), IgnoreAddressVerification.Value.ToString());
        if (CardIgnoreCVN != null) dict.Add(nameof(CardIgnoreCVN), CardIgnoreCVN.Value.ToString());
        if (PispRecipientReference != null) dict.Add(nameof(PispRecipientReference), PispRecipientReference);
        if (CardProcessorMerchantID != null) dict.Add(nameof(CardProcessorMerchantID), CardProcessorMerchantID);
        if (CustomerEmailAddress != null) dict.Add(nameof(CustomerEmailAddress), CustomerEmailAddress ?? string.Empty);
        if (NotificationEmailAddresses != null) dict.Add(nameof(NotificationEmailAddresses), NotificationEmailAddresses ?? string.Empty);
        if (Title != null) dict.Add(nameof(Title), Title);
        if (PartialPaymentSteps != null) dict.Add(nameof(PartialPaymentSteps), PartialPaymentSteps);

        return dict;
    }
}