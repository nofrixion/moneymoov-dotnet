//-----------------------------------------------------------------------------
// Filename: ReportTypesEnum.cs
// 
// Description: List of the types of reports supported.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 19 Dec 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReportTypesEnum
{
    Unknown = 0,

    /// <summary>
    /// A SWIFT MT940 Customer Statement report.
    /// </summary>
    SwiftCustomerStatement = 1,

    /// <summary>
    /// A customer activity report for MoneyMoov. 
    /// </summary>
    CustomerActivity = 2,

    /// <summary>
    /// A reconciliation report for safegaurded accounts.
    /// </summary>
    SafeGuardingReconciliation = 3,

    /// <summary>
    /// A report for the balance of all accounts for a merchant.
    /// </summary>
    MerchantAccountsBalance = 4,

    /// <summary>
    /// A report for the transactions for all accounts for a merchant.
    /// </summary>
    MerchantAccountsTransaction = 5,

    /// <summary>
    /// Custom report to export transactions for VisionBlue.
    /// </summary>
    VisionBlueTransaction = 6
}