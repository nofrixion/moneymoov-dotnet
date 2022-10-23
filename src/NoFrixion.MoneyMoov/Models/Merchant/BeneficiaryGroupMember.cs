//-----------------------------------------------------------------------------
// Filename: BeneficiaryGroupMember.cs
// 
// Description: Represents a member of a beneficiary group. Beneficiary groups
// are useful for cases where a business process requires a number of payouts
// to be created based on a known set of beneficiaries, e.g payroll.
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

public class BeneficiaryGroupMember
{
    public Guid ID { get; set; }

    /// <summary>
    /// The ID of the beneficiary for the membership.
    /// </summary>
    [Required]
    public Guid BeneficiaryID { get; set; }

    /// <summary>
    /// The ID of the beneficiary group for the membership.
    /// </summary>
    [Required]
    public Guid BeneficiaryGroupID { get; set; }

    /// <summary>
    /// Timestamp indicating when the group was created.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// The beneficiary for this membership.
    /// </summary>
    public Beneficiary? Beneficiary { get; set; }

    /// <summary>
    /// Places all the beneficiary group member's properties into a dictionary. Useful
    /// for testing when HTML form encoding is required.
    /// </summary>
    /// <returns>A dictionary with all the beneficiary's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(ID), ID.ToString());
        dict.Add(nameof(BeneficiaryID), BeneficiaryID.ToString());
        dict.Add(nameof(BeneficiaryGroupID), BeneficiaryGroupID.ToString());

        return dict;
    }
}