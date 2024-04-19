// -----------------------------------------------------------------------------
//  Filename: PayoutCreate.cs
// 
//  Description: Contains details of a payout request:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  05 Nov 2021  Arif Matin     Created, Carmichael House, Dublin, Ireland.
//  09 Nov 2021  Arif Matin     Changed Name to Payout.
//  10 Dec 2022  Aaron Clauson  Renamed to PayoutCreate.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PayoutCreate
{
    [Required(ErrorMessage = "AccountID is required.")]
    public Guid AccountID { get; set; }

    public AccountIdentifierType Type { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.00001, double.MaxValue,ErrorMessage = "Minimum value of 0.00001 is required for Amount")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or Sets the your reference property.
    /// </summary>
    public string? YourReference { get; set; }

    public string? TheirReference { get; set; }

    [Obsolete("Please use Destination.")]
    public Guid? DestinationAccountID
    {
        get => Destination?.AccountID;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.AccountID = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationIBAN
    {
        get => Destination?.Identifier?.IBAN;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.Identifier ??= new AccountIdentifierCreate { Currency = CurrencyTypeEnum.EUR };
            Destination.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountNumber
    {
        get => Destination?.Identifier?.AccountNumber;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.Identifier ??= new AccountIdentifierCreate { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.AccountNumber = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationSortCode
    {
        get => Destination?.Identifier?.SortCode;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.Identifier ??= new AccountIdentifierCreate { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.SortCode = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountName
    {
        get => Destination?.Name;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.Name = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public CounterpartyCreate? DestinationAccount
    {
        get => Destination;
        set => Destination = value;
    }

    public CounterpartyCreate? Destination { get; set; }

    /// <summary>
    /// Optional field to associate the payout with the invoice from an external 
    /// application such as Xero. The InvoiceID needs to be unique for each
    /// account.
    /// </summary>
    public string? InvoiceID { get; set; }

    /// <summary>
    /// If set to true the payout will get created even if the business validation 
    /// rules fail. The basic data validation rules must still pass. The original 
    /// purpose of this flag was to allow payouts to be created from i3rd party applications,
    /// such as Xero, that may not have things like an IBAN set for a supplier.
    /// The missing information must be filled, either by an update from the 3rd party
    /// application, or manually, before the payout can be submitted for processing.
    /// </summary>
    public bool AllowIncomplete { get; set; } = false;

    /// <summary>
    /// An optional list of tag ids to add to the payout.
    /// </summary>
    public List<Guid>? TagIds { get; set; }

    /// <summary>
    /// Should this payout be scheduled for a future date?
    /// </summary>
    public bool Scheduled { get; set; }
    
    /// <summary>
    /// The date the payout should be submitted.
    /// </summary>
    public DateTimeOffset? ScheduleDate { get; set; }

    /// <summary>
    /// For Bitcoin payouts, when this flag is set the network fee will be deducted from the send amount.
    /// THis is particularly useful for sweeps where it can be difficult to calculate the exact fee required.
    /// </summary>
    public bool BitcoinSubtractFeeFromAmount { get; set; }

    /// <summary>
    /// The Bitcoin fee rate to apply in Satoshis per virtual byte.
    /// </summary>
    public int BitcoinFeeSatsPerVbyte { get; set; }
    
    /// <summary>
    /// Optional. The ID of the beneficiary identifier to use for the payout destination.
    /// </summary>
    [Obsolete("Please use BeneficiaryID to set the beneficiary.")]
    public Guid? BeneficiaryIdentifierID { get; set; }

    /// <summary>
    /// Optional. The ID of the beneficiary to use for the payout destination.
    /// </summary>
    public Guid? BeneficiaryID { get; set; }

    /// <summary>
    /// Places all the payout's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payout's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>
        {
            { nameof(AccountID), AccountID.ToString() },
            { nameof(Type), Type.ToString() },
            { nameof(Description), Description ?? string.Empty },
            { nameof(Currency), Currency.ToString() },
            { nameof(Amount), Amount.ToString() },
            { nameof(YourReference), YourReference ?? string.Empty },
            { nameof(TheirReference), TheirReference ?? string.Empty },
            { nameof(InvoiceID), InvoiceID ?? string.Empty },
            { nameof(AllowIncomplete), AllowIncomplete.ToString() },
            { nameof(BeneficiaryID), BeneficiaryID?.ToString() ?? string.Empty }
        };

        if (Destination != null)
        {
            dict = dict.Concat(Destination.ToDictionary($"{nameof(Destination)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }
}
