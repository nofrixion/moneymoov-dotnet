// -----------------------------------------------------------------------------
//  Filename: AccountIdentifierCountrySpecificDetails.cs
// 
//  Description: Contains country specific details for types of accounts:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class CountrySpecificDetails
{
    /// <summary>
    /// The address of the destination bank.
    /// </summary>
    /// <value>The address of the destination bank.</value>
    public string BankAddress { get; set; }

    /// <summary>
    /// The code of the destination bank&#39;s branch.
    /// </summary>
    /// <value>The code of the destination bank&#39;s branch.</value>
    public string BankBranchCode { get; set; }

    /// <summary>
    /// The name of the destination bank&#39;s branch.
    /// </summary>
    /// <value>The name of the destination bank&#39;s branch.</value>
    public string BankBranchName { get; set; }

    /// <summary>
    /// The city in which the destination bank resides.
    /// </summary>
    /// <value>The city in which the destination bank resides.</value>
    public string BankCity { get; set; }

    /// <summary>
    /// The code identifying the target bank on its respective national network. This is not the BIC/SWIFT code. This is known as the &#39;ABA code&#39; in the U.S., &#39;ISFC&#39; in India, &#39;routing number&#39; in Canada, and so on.
    /// </summary>
    /// <value>The code identifying the target bank on its respective national network. This is not the BIC/SWIFT code. This is known as the &#39;ABA code&#39; in the U.S., &#39;ISFC&#39; in India, &#39;routing number&#39; in Canada, and so on.</value>
    public string BankCode { get; set; }

    /// <summary>
    /// The name of the destination bank.
    /// </summary>
    /// <value>The name of the destination bank.</value>
    public string BankName { get; set; }

    /// <summary>
    /// The type of the beneficiary. &#39;true&#39; for businesses, &#39;false&#39; otherwise.
    /// </summary>
    /// <value>The type of the beneficiary. &#39;true&#39; for businesses, &#39;false&#39; otherwise.</value>
    public bool Business { get; set; }

    /// <summary>
    /// The 18 digit identification code of the beneficiary. Applies to Chinese beneficiaries only.
    /// </summary>
    /// <value>The 18 digit identification code of the beneficiary. Applies to Chinese beneficiaries only.</value>
    public string ChineseId { get; set; }

    /// <summary>
    /// The province in which the beneficiary resides. Applies only to beneficiaries residing in China.
    /// </summary>
    /// <value>The province in which the beneficiary resides. Applies only to beneficiaries residing in China.</value>
    public string Province { get; set; }

    public override string ToString()
    {
        return $"{nameof(BankAddress)}: {BankAddress}, {nameof(BankBranchCode)}: {BankBranchCode}, {nameof(BankBranchName)}: {BankBranchName}, {nameof(BankCity)}: {BankCity}, {nameof(BankCode)}: {BankCode}, {nameof(BankName)}: {BankName}, {nameof(Business)}: {Business}, {nameof(ChineseId)}: {ChineseId}, {nameof(Province)}: {Province}";
    }
}