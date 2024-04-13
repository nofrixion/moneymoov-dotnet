//-----------------------------------------------------------------------------
// Filename: OptionalEmailAddressAttribute.cs
//
// Description: Data modela attribute that treats an empty string the same
// as a null value for an email address.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 11 Apr 2024  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov;

public class OptionalEmailAddressAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrEmpty(value.ToString()))
        {
            return ValidationResult.Success!;
        }

        var emailAddressAttribute = new EmailAddressAttribute();
        if (emailAddressAttribute.IsValid(value))
        {
            return ValidationResult.Success!;
        }
        return new ValidationResult("The email address is not in a correct format.");
    }
}
