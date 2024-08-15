// -----------------------------------------------------------------------------
//  Filename: PermissionGroupMerchantPermissions.cs
// 
//  Description: Represents the permissions for a merchant in a permission group:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  15 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.Common.Permissions;

namespace NoFrixion.MoneyMoov.Models.Permissions;

public class PermissionGroupMerchantPermissionsCreate
{
    [Required]
    public Guid MerchantID { get; set; }
    
    [Required]
    public MerchantPermissions Permissions { get; set; }
}