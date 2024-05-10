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

namespace NoFrixion.MoneyMoov;

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
    /// A report for merchant account balances.
    /// </summary>
    MerchantAccountBalances = 4,
}