// -----------------------------------------------------------------------------
//  Filename: RoleUser.cs
// 
//  Description: Contains the role user model.:
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

public class RoleUser
{
    public Guid ID { get; set; }

    public Guid RoleID { get; set; }

    public Guid UserID { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public bool IsEnabled { get; set; }

    public List<RoleUserAccount>? Accounts { get; set; }

    public List<RoleUserMerchant>? Merchants { get; set; }

    public User? User { get; set; }
    
    public Role? Role { get; set; }
}