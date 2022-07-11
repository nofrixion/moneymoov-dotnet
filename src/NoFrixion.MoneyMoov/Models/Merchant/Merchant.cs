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
    public string? ModulrNotificationID { get; set; }
    public string? ModulrCustomerID { get; set; }
    public string? EmailAddress { get; set; }
}
