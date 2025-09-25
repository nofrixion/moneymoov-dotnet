// -----------------------------------------------------------------------------
//  Filename: IPAddressAttribute.cs
// 
//  Description: A custom attribute to validate multiple IP addresses:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  10 09 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class IPAddressAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string ipAddressesValue)
        {
            return new ValidationResult($"The {validationContext.DisplayName} field must be a string.");
        }

        if (string.IsNullOrWhiteSpace(ipAddressesValue))
        {
            return ValidationResult.Success;
        }

        var ipAddresses = ipAddressesValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var ipAddress in ipAddresses)
        {
            if (!System.Net.IPAddress.TryParse(ipAddress, out _) && !System.Net.IPNetwork.TryParse(ipAddress, out _))
            {
                return new ValidationResult($"The {validationContext.DisplayName} field contains an invalid IP address: {ipAddress}");
            }
        }
        
        return ValidationResult.Success;
    }
}