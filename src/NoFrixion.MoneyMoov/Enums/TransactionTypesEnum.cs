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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov;

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
    UK_BACS = 4,

    /// <summary>
    /// A reversal of a previous payout.
    /// </summary>
    [Display(Name = "Reversal")]
    Reversal = 5,

    [Display(Name = "Bitcoin")]
    BTC = 6,

    [Display(Name = "Bitcoin Testnet")]
    BTC_TEST = 7,

    [Display(Name = "Lightning Bitcoin")]
    LBTC = 8,

    [Display(Name = "Lightning Bitcoin Testnet")]
    LBTC_TEST = 9,
    
    /// <summary>
    /// The European Union, SEPA Direct Debit network.
    /// </summary>
    [Display(Name = "SEPA Direct Debit")]
    SEPA_DD = 10,

    /// <summary>
    /// Payment was sent, or received, over a SEPA scheme but the exact one is unknown.
    /// </summary>
    [Display(Name = "SEPA")]
    SEPA = 11,

    /// <summary>
    /// The transaction type was not recognised.
    /// </summary>
    [Display(Name = "Unknown type")]
    Unknown = 12,

    [Display(Name = "TARGET2")]
    TARGET2 = 13,
    
    [Display(Name = "SEPA Direct Debit Rejection / Chargeback")]
    SEPA_DD_REJECT = 14,

    [Display(Name = "Cross Border")]
    CROSS_BORDER = 15,

    /// <summary>
    /// The UK, Clearing House Automated Payment System network.
    /// </summary>
    UK_CHAPS = 16
}
