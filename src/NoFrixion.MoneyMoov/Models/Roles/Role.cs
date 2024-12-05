// -----------------------------------------------------------------------------
//  Filename: Role.cs
// 
//  Description: Contains the role model.:
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

using NoFrixion.Common.Permissions;

namespace NoFrixion.MoneyMoov.Models.Roles;

public class Role
{
    public Guid ID { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid MerchantID { get; set; }

    public List<string> MerchantPermissions { get; set; } = null!;

    public List<string> AccountPermissions { get; set; } = null!;

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public List<RoleUser>? Users { get; set; }
}