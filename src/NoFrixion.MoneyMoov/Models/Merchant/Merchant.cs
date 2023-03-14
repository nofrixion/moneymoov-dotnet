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
    /// Unique ID for the merchant.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// The registered business name of the merchant.
    /// </summary>
    public string? Name { get; set; }

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
    /// The list of users that have been assigned a role on the merchant.
    /// </summary>
    public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// An optional list of descriptive tags that can be used on merchant entities
    /// such as payment requests.
    /// </summary>
    public List<Tag> Tags { get; set; } = new List<Tag>();

    /// <summary>
    /// Gets the most appropriate display name for the merchant which
    /// means use the trading name if set and if not, default to the business name.
    /// </summary>
    /// <returns></returns>
    public string GetDisplayName()
        => !string.IsNullOrEmpty(TradingName) ? TradingName : Name ?? string.Empty;
}
