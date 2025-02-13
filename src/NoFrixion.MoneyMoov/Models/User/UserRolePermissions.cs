// -----------------------------------------------------------------------------
//  Filename: UserRolePermissions.cs
// 
//  Description: The business layer representation of a user's permissions.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  23 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserRolePermissions
{
    public Dictionary<string, List<string>> MerchantPermissions { get; set; } = new();
    
    public Dictionary<string, List<string>> AccountPermissions { get; set; } = new();
}