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

    [Obsolete("Please use DestinationAccount.")]
    public Guid? DestinationAccountID
    {
        get => DestinationAccount?.AccountID;
        set
        {
            DestinationAccount ??= new Counterparty();
            DestinationAccount.AccountID = value;
        }
    }

    [Obsolete("Please use DestinationAccount.")]
    public string? DestinationIBAN
    {
        get => DestinationAccount?.Identifier?.IBAN;
        set
        {
            DestinationAccount ??= new Counterparty();
            DestinationAccount.Identifier ??= new AccountIdentifier();
            DestinationAccount.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use DestinationAccount.")]
    public string? DestinationAccountNumber
    {
        get => DestinationAccount?.Identifier?.AccountNumber;
        set
        {
            DestinationAccount ??= new Counterparty();
            DestinationAccount.Identifier ??= new AccountIdentifier();
            DestinationAccount.Identifier.AccountNumber = value;
        }
    }

    [Obsolete("Please use DestinationAccount.")]
    public string? DestinationSortCode
    {
        get => DestinationAccount?.Identifier?.SortCode;
        set
        {
            DestinationAccount ??= new Counterparty();
            DestinationAccount.Identifier ??= new AccountIdentifier();
            DestinationAccount.Identifier.SortCode = value;
        }
    }

    [Obsolete("Please use DestinationAccount.")]
    public string? DestinationAccountName
    {
        get => DestinationAccount?.Name;
        set
        {
            DestinationAccount ??= new Counterparty();
            DestinationAccount.Name = value;
        }
    }

    public Counterparty? DestinationAccount { get; set; }

    /// <summary>
    /// If set to true the payout will get updated even if the business validation 
    /// rules fail. The basic data validation rules must still pass. The original 
    /// purpose of this flag was to allow payouts to be created from i3rd party applications,
    /// such as Xero, that may not have things like an IBAN set for a supplier.
    /// The missing information must be filled, either by an update from the 3rd party
    /// application, or manually, before the payout can be submitted for processing.
    /// </summary>
    public bool? AllowIncomplete { get; set; }

    /// <summary>
    /// Places all the payout's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payout's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (AccountID != null) dict.Add(nameof(AccountID), AccountID.Value.ToString());
        if (Type != null) dict.Add(nameof(Type), Type.Value.ToString());
        if (Description != null) dict.Add(nameof(Description), Description.ToString());
        if (Amount != null) dict.Add(nameof(Amount), Amount.Value.ToString());
        if (YourReference != null) dict.Add(nameof(YourReference), YourReference.ToString());
        if (TheirReference != null) dict.Add(nameof(TheirReference), TheirReference.ToString());
        if (AllowIncomplete != null) dict.Add(nameof(AllowIncomplete), AllowIncomplete.Value.ToString());

        if (DestinationAccount != null)
        {
            dict = dict.Concat(DestinationAccount.ToDictionary($"{nameof(DestinationAccount)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }
}