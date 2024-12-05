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

public class RoleCreate : IValidatableObject
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public Guid MerchantID { get; set; }
    
    [Required]
    public List<string> MerchantPermissions { get; set; } = new();

    [Required]
    public List<string> AccountPermissions { get; set; } = new();
    
    public List<RoleUserCreate>? Users { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (AccountPermissions.Any(p => !Enum.TryParse<AccountPermissions>(p, out _)))
        {
            var validAccountPermissions = string.Join(", ", Enum.GetValues(typeof(AccountPermissions)).Cast<AccountPermissions>());
            
            yield return new ValidationResult($"Invalid account permission. Valid permissions are {validAccountPermissions}", [nameof(AccountPermissions)]);
        }
        
        if (MerchantPermissions.Any(p => !Enum.TryParse<MerchantPermissions>(p, out _)))
        {
            var validMerchantPermissions = string.Join(", ", Enum.GetValues(typeof(MerchantPermissions)).Cast<MerchantPermissions>());
            
            yield return new ValidationResult($"Invalid merchant permission. Valid permissions are {validMerchantPermissions}", [nameof(MerchantPermissions)]);
        }
    }
}