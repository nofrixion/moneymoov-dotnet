// -----------------------------------------------------------------------------
//  Filename: RoleCreate.cs
// 
//  Description: Model for creating a new role:
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

using System.ComponentModel.DataAnnotations;
using NoFrixion.Common.Permissions;

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleCreate
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public Guid MerchantID { get; set; }

    [Required]
    public UserPermissions UserPermissions { get; set; }

    [Required]
    public MerchantPermissions MerchantPermissions { get; set; }

    [Required]
    public AccountPermissions AccountPermissions { get; set; }
    
    public List<RoleUserCreate>? Users { get; set; }
}