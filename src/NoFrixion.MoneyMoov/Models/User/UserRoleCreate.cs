// -----------------------------------------------------------------------------
//  Filename: UserRoleCreate.cs
// 
//  Description: Used when assigning a user role to a merchant.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  12 08 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserRoleCreate
{
    public Guid MerchantID { get; set; }

    public string? EmailAddress { get; set; }

    public UserRolesEnum UserRole { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MerchantID == Guid.Empty)
        {
            yield return new ValidationResult($"{nameof(MerchantID)} is missing or invalid");
        }

        if (string.IsNullOrEmpty(EmailAddress))
        {
            yield return new ValidationResult($"{nameof(EmailAddress)} is missing or invalid");
        }
    }

}