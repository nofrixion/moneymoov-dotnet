// -----------------------------------------------------------------------------
//  Filename: TransactionTypesEnum.cs
// 
//  Description: List of the types of transactions that can recorded on the
//  MoneyMoov ledger, represented by the Transactions table.
//
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(StringEnumConverter))]
public enum TransactionTypesEnum
{
    /// <summary>
    /// An internal transaction between accounts using the same ledger.
    /// </summary>
    [Display(Name = "Internal")]
    Internal = 0,

    /// <summary>
    /// The European Union, SEPA Credit Transfer settlement network.
    /// </summary>
    [Display(Name = "SEPA Credit Transfer")]
    SEPA_CT = 1,

    /// <summary>
    /// The European Union, SEPA Instant settlement network.
    /// </summary>
    [Display(Name = "SEPA Instant")]
    SEPA_INST = 2,

    /// <summary>
    /// The UK, Faster Payments settlement network.
    /// </summary>
    [Display(Name = "Faster Payments")]
    UK_FAST = 3,

    /// <summary>
    /// The UK, Bankers Automated Clearing House network.
    /// </summary>
    [Display(Name = "Bankers Automated Clearing House")]
    UK_BACS = 4
}
