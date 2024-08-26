// -----------------------------------------------------------------------------
//  Filename: RoleUserAccount.cs
// 
//  Description: Contains the role user account model.:
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

using NoFrixion.MoneyMoov.Models.OpenBanking;

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUserAccount
{
    public Guid ID { get; set; }

    public Guid RoleUserID { get; set; }

    public Guid AccountID { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public PaymentAccount? Account { get; set; }
}