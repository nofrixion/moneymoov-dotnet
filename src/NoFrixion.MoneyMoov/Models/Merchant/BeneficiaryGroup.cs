//-----------------------------------------------------------------------------
// Filename: BeneficiaryGroup.cs
// 
// Description: Represents a collection of beneficiaries. Useful for cases
// where a business process requires a number of payouts to be created based
// on a known set of beneficiaries, e.g payroll.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 23 Oct 2022  Aaron Clauson   Created, stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class BeneficiaryGroup
{
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets the merchant id.
    /// </summary>
    [Required]
    public Guid MerchantID { get; set; }

    /// <summary>
    /// The descriptive name for the beneficiary group.
    /// </summary>
    [Required(ErrorMessage = "THe Beneficiary Group Name is required.")]
    public string? Name { get; set; }

    /// <summary>
    /// Places all the beneficiary group's properties into a dictionary. Useful for testing
    /// when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the beneficiary's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(ID), ID.ToString());
        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(Name), Name ?? string.Empty);

        return dict;
    }
}