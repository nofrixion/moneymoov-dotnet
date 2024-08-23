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
    public Common.Permissions.UserPermissions UserPermissions { get; set; }
    
    public List<UserMerchantPermissions> MerchantPermissions { get; set; } = new();
    
    public List<UserAccountPermissions> AccountPermissions { get; set; } = new();
}

public class UserMerchantPermissions
{
    public Guid MerchantID { get; set; }
    
    public Common.Permissions.MerchantPermissions Permissions { get; set; }
}

public class UserAccountPermissions
{
    public Guid AccountID { get; set; }
    
    public Common.Permissions.AccountPermissions Permissions { get; set; }
}