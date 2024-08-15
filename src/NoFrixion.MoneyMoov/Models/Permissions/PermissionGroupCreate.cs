// -----------------------------------------------------------------------------
//  Filename: PermissionGroupCreate.cs
// 
//  Description: Represents the model to create a new PermissionGroup:
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

public class PermissionGroupCreate
{
    [Required]
    public required string Name { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    [Required] 
    public required Guid MerchantID { get; set; }
    
    public List<Guid>? Users { get; set; } = [];

    public UserPermissions? UserPermissions { get; set; }
    
    public List<PermissionGroupMerchantPermissionsCreate>? MerchantPermissions { get; set; }
    
    public List<PermissionGroupAccountPermissionsCreate>? AccountPermissions { get; set; }
}