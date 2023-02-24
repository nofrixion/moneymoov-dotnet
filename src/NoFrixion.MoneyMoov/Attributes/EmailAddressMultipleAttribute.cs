// -----------------------------------------------------------------------------
//  Filename: EmailAddressMultipleAttribute.cs
// 
//  Description: A custom attribute to validate multiple emails addresses:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 02 2023  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class EmailAddressMultipleAttribute : DataTypeAttribute
{
    private readonly EmailAddressAttribute _emailAddressAttribute = new();

    public EmailAddressMultipleAttribute() : base(DataType.EmailAddress) { }
    
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }

        if (value is not string emailAddresses)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(emailAddresses))
        {
            return false;
        }

        var emails = emailAddresses.Split(new[] { ';', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        return emails.All(_emailAddressAttribute.IsValid);
    }
}