// -----------------------------------------------------------------------------
//  Filename: PermissionsValidator.cs
// 
//  Description: Contains the permissions validator class.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  05 12 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.Common.Permissions;

namespace NoFrixion.MoneyMoov.Models.Roles;

public class PermissionsValidator
{
    public static IEnumerable<ValidationResult> Validate(List<string>? accountPermissions, List<string>? merchantPermissions, ValidationContext validationContext)
    {
        if (accountPermissions != null && accountPermissions.Any(p => !Enum.TryParse<AccountPermissions>(p, out _)))
        {
            var validAccountPermissions = string.Join(", ", Enum.GetValues(typeof(AccountPermissions)).Cast<AccountPermissions>());
            
            yield return new ValidationResult($"Invalid account permission. Valid permissions are {validAccountPermissions}", [nameof(AccountPermissions)]);
        }
        
        if (merchantPermissions != null && merchantPermissions.Any(p => !Enum.TryParse<MerchantPermissions>(p, out _)))
        {
            var validMerchantPermissions = string.Join(", ", Enum.GetValues(typeof(MerchantPermissions)).Cast<MerchantPermissions>());
            
            yield return new ValidationResult($"Invalid merchant permission. Valid permissions are {validMerchantPermissions}", [nameof(MerchantPermissions)]);
        }
    }
}