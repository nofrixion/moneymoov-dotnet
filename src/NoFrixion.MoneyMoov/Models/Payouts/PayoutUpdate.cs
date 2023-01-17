// -----------------------------------------------------------------------------
//  Filename: PayoutUpdate.cs
// 
//  Description: Contains details of a payout update request:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  22 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PayoutUpdate
{
    public Guid? AccountID { get; set; }

    public AccountIdentifierType? Type { get; set; }

    public string? Description { get; set; }

    public CurrencyTypeEnum? Currency { get; set; }

    public decimal? Amount { get; set; }

    public string? YourReference { get; set; }

    public string? TheirReference { get; set; }

    public Guid? DestinationAccountID { get; set; }

    public string? DestinationIBAN { get; set; }

    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public string? DestinationAccountName { get; set; }
}