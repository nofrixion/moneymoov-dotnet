//-----------------------------------------------------------------------------
// Filename: Merchant.cs
// 
// Description: Merchant class for MerchantEntity:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
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
    public Guid ID { get; set; }
    public string? Name { get; set; }
    public bool Enabled { get; set; }
    
    /// <summary>
    /// The Company ID recorded in the Compliance system.
    /// </summary>
    public Guid? CompanyID { get; set; }

    public string? MerchantCategoryCode { get; set; }
    public string? ShortName { get; set; }
    public string? ModulrCustomerID { get; set; }
    public string? EmailAddress { get; set; }

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
    /// Geets the most appropriate display name for the merchant which
    /// means use the trading name if set and if not, default to the business name.
    /// </summary>
    /// <returns></returns>
    public string GetDisplayName()
        => !string.IsNullOrEmpty(TradingName) ? TradingName : Name ?? string.Empty;
}
