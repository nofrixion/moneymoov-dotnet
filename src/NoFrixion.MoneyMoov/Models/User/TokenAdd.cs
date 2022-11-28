// -----------------------------------------------------------------------------
//  Filename: TokenAdd.cs
// 
//  Description: Used when adding a new User Token:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  25 11 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using LanguageExt;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class TokenAdd : IValidatableObject
{
    /// <summary>
    /// The merchant id to add to the token
    /// </summary>
    [Required(ErrorMessage = "MerchantID is required")]
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Token description
    /// </summary>
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MerchantID == Guid.Empty)
        {
            yield return new ValidationResult($"{nameof(MerchantID)} is missing or invalid");
        }

        if (string.IsNullOrEmpty(Description))
        {
            yield return new ValidationResult($"{nameof(Description)} is missing or invalid");
        }

        if (!ValidateDescription())
        {
            yield return new ValidationResult("{nameof(Description) can not have any special characters");
        }
    }

    private bool ValidateDescription()
    {
        Regex regex = new Regex(@"^[a-zA-Z0-9_\s]*$");

        if (regex.IsMatch(this.Description))
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Places all the token add member's properties into a dictionary. Useful
    /// for testing when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the beneficiary's non-collection properties 
    /// represented as key-value pairs.</returns>
    public IReadOnlyDictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(Description), Description.ToString());

        return dict;
    }
}