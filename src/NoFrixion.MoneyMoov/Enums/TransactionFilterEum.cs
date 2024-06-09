//-----------------------------------------------------------------------------
// Filename: TransactionCreditTypesEnum.cs
// 
// Description: A list of the types of credit filters that can be used when requesting
// the transactions for an account.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// New Year's Day 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionCreditTypesEnum
{
    /// <summary>
    /// Retrieve all transactions.
    /// </summary>
    All,

    /// <summary>
    /// Retrieve only pay in (credit) transactions.
    /// </summary>
    Payin,

    /// <summary>
    /// Retrieve only pay out (debit) transactions.
    /// </summary>
    Payout
}