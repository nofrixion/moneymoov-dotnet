//-----------------------------------------------------------------------------
// Filename: Merchant.cs
// 
// Description: Model to represent a MoneyMoov merchant. A merchant is the top
// level entity that is the ultimate parent of all other resources such as
// payment accounts etc.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 06 Dec 2021     Created, Belvedere Road, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class Merchant
{
    /// <summary>
    /// The original version of the merchant notifications. Only supported PAYIN and PAYOUT notifications and
    /// they were sent as unwrapped transaction models, i.e. no parent WebhookEvent model was used.
    /// </summary>
    public const int WEBHOOK_VERSION_ONE = 1;

    /// <summary>
    /// The webhook version at the point the additional PaymentRequest, Payout, Rule and Transaction webhook
    /// options were added. This version sent webhooks wrapped in a parent WebhookEvent model.
    /// </summary>
    public const int WEBHOOK_VERSION_TWO = 2;

    /// <summary>
    /// Unique ID for the merchant.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// The registered business name of the merchant.
    /// </summary>
    public string? Name { get; set; }

    [Obsolete("This property has been deprecated.")]
    public bool Enabled { get; set; }

    /// <summary>
    /// The Company ID recorded in the Compliance system.
    /// </summary>
    public Guid? CompanyID { get; set; }

    /// <summary>
    /// The industry code that represents the merchant's primary trading activity.
    /// </summary>
    public string? MerchantCategoryCode { get; set; }

    /// <summary>
    /// A URL friendly shortish name for the merchant. Principal purpose is
    /// to use in the hosted payment page URL.
    /// </summary>
    public string? ShortName { get; set; }

    /// <summary>
    /// An optional trading name. If not set the Name field will be used .
    /// </summary>
    public string? TradingName { get; set; }

    /// <summary>
    /// The maximum number of payment accounts that can be created for the Merchant.
    /// To increase the limit contact support.
    /// </summary>
    public int PaymentAccountLimit { get; set; }

    /// <summary>
    /// Timestamp the merchant was added to MoneyMoov.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// The jurisdiction the merchant entity is incorporated or established in.
    /// </summary>
    public JurisdictionEnum Jurisdiction { get; set; }

    /// <summary>
    /// The version of the hosted payment page to use with the merchant.
    /// </summary>
    public int HostedPayVersion { get; set; }

    /// <summary>
    /// The maximum number of web hooks that can be created for the Merchant.
    /// To increase the limit contact support.
    /// </summary>
    public int WebHookLimit { get; set; }

    /// <summary>
    /// Indicates if a QR Code containing the payment link should be displayed
    /// on the hosted payment page.
    /// </summary>
    public bool DisplayQrOnHostedPay { get; set; }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [Obsolete("Modulr is no longer used.")]
    public string? ModulrCustomerID { get; set; }

    /// <summary>
    /// The payment methods that are configured and supported for this merchant.
    /// Returned as a comma-separated list of PaymentMethodTypeEnum values.
    /// </summary>
    [Obsolete("Please usse SupportedPaymentMethodsList instead.")]
    public PaymentMethodTypeEnum SupportedPaymentMethods { get; set; }

    /// <summary>
    /// The payment methods that are configured and supported for this merchant.
    /// </summary>
    public List<PaymentMethodTypeEnum> SupportedPaymentMethodsList { get; set; } = new List<PaymentMethodTypeEnum>();

    // /// <summary>
    // /// The role of the identity that loaded the merchant record.
    // /// </summary>
    // public UserRolesEnum YourRole { get; set; } = UserRolesEnum.NewlyRegistered;
    //
    /// <summary>
    /// The name of the role for the identity that loaded the merchant record.
    /// </summary>
    public string? YourRoleName { get; set; }

    /// <summary>
    /// An optional list of descriptive tags that can be used on merchant entities
    /// such as payment requests.
    /// </summary>
    public List<Tag> Tags { get; set; } = new List<Tag>();

    public List<PaymentAccount> PaymentAccounts { get; set; } = new List<PaymentAccount>();

    /// <summary>
    /// The list of currencies that the merchant has accounts for.
    /// </summary>
    public List<CurrencyTypeEnum> AccountCurrencies { get; set; } = [];

    /// <summary>
    /// Gets the most appropriate display name for the merchant which
    /// means use the trading name if set and if not, default to the business name.
    /// </summary>
    /// <returns></returns>
    public string GetDisplayName()
        => !string.IsNullOrEmpty(TradingName) ? TradingName : Name ?? string.Empty;
}
