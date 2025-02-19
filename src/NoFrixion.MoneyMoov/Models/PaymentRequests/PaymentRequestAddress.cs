//-----------------------------------------------------------------------------
// Filename: PaymentRequestAddress.cs
//
// Description: Represents a shipping or billing address for a payer.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 17 Jan 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestAddress : IValidatableObject
{
    public const string DEFAULT_COUNTRY_CODE = "IE";

    public Guid? ID { get; set; }

    public Guid? PaymentRequestID {  get; set; }

    public AddressTypesEnum AddressType { get; set; } = AddressTypesEnum.Unknown;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressCity { get; set; }

    public string? AddressCounty { get; set; }

    public string? AddressPostCode { get; set; }

    public string? AddressCountryCode { get; set; }

    public string? Phone { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(AddressLine1))
        {
            yield return new ValidationResult($"{nameof(AddressLine1)} must be provided.", new string[] { nameof(AddressLine1) });
        }

        if (!string.IsNullOrEmpty(AddressCountryCode) && AddressCountryCode.Length != 2)
        {
            yield return new ValidationResult($"{nameof(AddressCountryCode)} must be 2 characters.", new string[] { nameof(AddressCountryCode) });
        }
    }

    /// <summary>
    /// Places all the payment address's properties into a dictionary. Useful for testing
    /// when HTML form encoding is required.
    /// </summary>
    /// <param name="includeIDs">If true the identity properties will be included in the dictionary.</param>
    /// <returns>A dictionary with all the payment request's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary(bool includeIDs)
    {
        var dict = new Dictionary<string, string>();

        if(includeIDs)
        {
            dict.Add(nameof(ID), ID.GetValueOrDefault().ToString());
            dict.Add(nameof(PaymentRequestID), PaymentRequestID.GetValueOrDefault().ToString());
        }

        dict.Add(nameof(AddressType), AddressType.ToString());
        dict.Add(nameof(FirstName), FirstName ?? string.Empty);
        dict.Add(nameof(LastName), LastName ?? string.Empty);
        dict.Add(nameof(AddressLine1), AddressLine1 ?? string.Empty);
        dict.Add(nameof(AddressLine2), AddressLine2 ?? string.Empty);
        dict.Add(nameof(AddressCity), AddressCity ?? string.Empty);
        dict.Add(nameof(AddressCounty), AddressCounty ?? string.Empty);
        dict.Add(nameof(AddressPostCode), AddressPostCode ?? string.Empty);
        dict.Add(nameof(AddressCountryCode), AddressCountryCode ?? string.Empty);
        dict.Add(nameof(Phone), Phone ?? string.Empty);
        dict.Add(nameof(Email), Email ?? string.Empty);

        return dict;
    }
    
    public string ToDisplayString()
    {
        var sb = new StringBuilder();
        if(!string.IsNullOrWhiteSpace(AddressLine1))
        {
            sb.Append(AddressLine1);
        }
        
        if(!string.IsNullOrWhiteSpace(AddressLine2))
        {
            sb.Append(", ");
            sb.Append(AddressLine2);
        }
        
        if(!string.IsNullOrWhiteSpace(AddressCity))
        {
            sb.Append(", ");
            sb.Append(AddressCity);
        }
        
        if(!string.IsNullOrWhiteSpace(AddressCounty))
        {
            sb.Append(", ");
            sb.Append(AddressCounty);
        }
        
        if(!string.IsNullOrWhiteSpace(AddressPostCode))
        {
            sb.Append(", ");
            sb.Append(AddressPostCode);
        }
        
        if(!string.IsNullOrWhiteSpace(AddressCountryCode))
        {
            sb.Append(", ");
            sb.Append(AddressCountryCode);
        }
        
        return sb.ToString();
    }
}
