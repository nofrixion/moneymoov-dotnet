// -----------------------------------------------------------------------------
//  Filename: IPayout.cs
// 
//  Description: Payout interface:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  22 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public interface IPayout
{
    /// <summary>
    /// Gets or Sets guid ID for payout
    /// </summary>
    Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets Account Id of sending account
    /// </summary>
    Guid AccountID { get; set; }

    AccountIdentifierType Type { get; set; }

    /// <summary>
    /// Gets or Sets description of payout request
    /// </summary>
    string Description { get; set; }

    /// <summary>
    /// Gets or Sets Currency of payout request
    /// </summary>
    CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Gets or Sets payout amount
    /// </summary>
    decimal Amount { get; set; }

    /// <summary>
    /// Gets or Sets your reference ID
    /// </summary>
    string YourReference { get; set; }

    /// <summary>
    /// Gets or Sets destination reference ID
    /// </summary>
    string TheirReference { get; set; }

    Guid DestinationAccountID { get; set; }

    string DestinationIBAN { get; set; }

    string DestinationAccountNumber { get; set; }

    string DestinationSortCode { get; set; }

    string DestinationAccountName { get; set; }
}