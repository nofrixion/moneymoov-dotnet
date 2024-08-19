// -----------------------------------------------------------------------------
//  Filename: RoleUserMerchant.cs
// 
//  Description: Contains the role user merchant model.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  19 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUserMerchant
{
    public Guid ID { get; set; }

    public Guid RoleUserID { get; set; }

    public Guid MerchantID { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    // public Merchant? Merchant { get; set; }
}