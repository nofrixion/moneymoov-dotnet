//-----------------------------------------------------------------------------
// Filename: PaymentRequestConstants.cs
//
// Description: Constants related to payment request. Contains constants for validation messages and regexes.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 14 Jun 2023  Saurav Maiti       Created, Harcourt Street, Dublin, Ireland.
//
// License: MIT
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public class PaymentRequestConstants
{
    public const string CUSTOMER_ID_CHARS_REGEX = @"[a-zA-Z0-9\-]+";

    public const string CUSTOMER_ID_ERROR_MESSAGE =
        @"The CustomerID can only contain alphanumeric characters and dash.";

    public const string ORDER_ID_CHARS_REGEX = @"[a-zA-Z0-9\-_\.@&\*%\$#!:; ]+";

    public const string ORDER_ID_ERROR_MESSAGE =
        @"The OrderID can only contain alphanumeric characters and -_.@&*%$#!:; and space.";

    public const string SHIPPING_FIRST_NAME_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_FIRST_NAME_ERROR_MESSAGE =
        @"The ShippingFirstName had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_LAST_NAME_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_LAST_NAME_ERROR_MESSAGE =
        @"The ShippingLastName had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_ADDRESS_LINE_1_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_ADDRESS_LINE_1_ERROR_MESSAGE =
        @"The ShippingAddressLine1 had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_ADDRESS_LINE_2_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_ADDRESS_LINE_2_ERROR_MESSAGE =
        @"The ShippingAddressLine2 had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_CITY_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_CITY_ERROR_MESSAGE =
        @"The ShippingAddressCity had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_COUNTY_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_COUNTY_ERROR_MESSAGE =
        @"The ShippingAddressCounty had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_POSTAL_CODE_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_POSTAL_CODE_ERROR_MESSAGE =
        @"The ShippingAddressPostCode had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_COUNTRY_CODE_CHARS_REGEX = @"[^\<\>]+";

    public const string SHIPPING_COUNTRY_CODE_ERROR_MESSAGE =
        @"The ShippingAddressCountryCode had an invalid character. It cannot contain < or >.";

    public const string SHIPPING_PHONE_CHARS_REGEX = @"[0-9\+\- ]+";

    public const string SHIPPING_PHONE_ERROR_MESSAGE =
        @"The ShippingAddressPhone had an invalid character. It can only contain numbers, +, - and space.";

    public const string CARD_PROCESSOR_MERCHANT_ID_CHARS_REGEX = @"[a-zA-Z0-9]+";

    public const string CARD_PROCESSOR_MERCHANT_ID_ERROR_MESSAGE =
        @"The CardProcessorMerchantID can only contain alphanumeric characters.";

    public const string NOTIFICATION_EMAIL_ADDRESSES_ERROR_MESSAGE =
        "One or more of the email addresses are invalid. Addresses can be separated by a comma, semi-colon or space.";
    
    /// <summary>
    /// This is the display name for the card payment method. It is used to
    /// display the payment method in receipts and other user interfaces.
    /// </summary>
    public const string DISPLAY_CARD_PAYMENT_METHOD = "Card";
    
    /// <summary>
    /// This is the display name for the direct debit payment method. It is used to
    /// display the payment method in receipts and other user interfaces.
    /// </summary>
    public const string DISPLAY_DIRECT_DEBIT_PAYMENT_METHOD = "Direct debit";
    
}