// -----------------------------------------------------------------------------
//  Filename: RoleUpdate.cs
// 
//  Description: Role update model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  26 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUpdate : IValidatableObject
{
    /// <summary>
    /// The role id
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// The role name
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The role description
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The roles merchant permissions
    /// </summary>
    public List<string>? MerchantPermissions { get; set; }

    /// <summary>
    /// The roles account permissions
    /// </summary>
    public List<string>? AccountPermissions { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return PermissionsValidator.Validate(AccountPermissions, MerchantPermissions, validationContext);
    }
}