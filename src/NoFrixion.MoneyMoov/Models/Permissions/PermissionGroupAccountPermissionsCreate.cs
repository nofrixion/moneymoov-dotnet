// -----------------------------------------------------------------------------
//  Filename: PermissionGroupAccountPermissions.cs
// 
//  Description: Represents the permissions for an account in a permission group:
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

public class PermissionGroupAccountPermissionsCreate
{
    [Required]
    public Guid AccountID { get; set; }
    
    [Required]
    public AccountPermissions Permissions { get; set; }
}