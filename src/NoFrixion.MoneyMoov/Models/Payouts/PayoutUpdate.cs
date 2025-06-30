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
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PayoutUpdate
{
    /// <summary>
    /// The ID of the source account for the payout.
    /// </summary>
    public Guid? AccountID { get; set; }

    public AccountIdentifierType? Type { get; set; }

    public string? Description { get; set; }

    public CurrencyTypeEnum? Currency { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Minimum value of 0.01 is required for Amount.")]
    public decimal? Amount { get; set; }

    public string? YourReference { get; set; }

    public string? TheirReference { get; set; }

    [Obsolete("Please use Destination.")]
    public Guid? DestinationAccountID
    {
        get => Destination?.AccountID;
        set
        {
            Destination ??= new Counterparty();
            Destination.AccountID = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationIBAN
    {
        get => Destination?.Identifier?.IBAN;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.EUR };
            Destination.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountNumber
    {
        get => Destination?.Identifier?.AccountNumber;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.AccountNumber = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationSortCode
    {
        get => Destination?.Identifier?.SortCode;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.SortCode = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountName
    {
        get => Destination?.Name;
        set
        {
            Destination ??= new Counterparty();
            Destination.Name = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public Counterparty? DestinationAccount
    {
        get => Destination;
        set => Destination = value;
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationBitcoinAddress
    {
        get => Destination?.Identifier?.BitcoinAddress;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.BTC };
            Destination.Identifier.BitcoinAddress = value;
        }
    }

    public Counterparty? Destination { get; set; }

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
    /// An optional list of tag ids to add to the payout.
    /// </summary>
    public List<Guid>? TagIds { get; set; }

    /// <summary>
    /// Should this payout be scheduled for a future date?
    /// </summary>
    public bool? Scheduled { get; set; }
    
    /// <summary>
    /// The date the payout should be submitted.
    /// </summary>
    public DateTimeOffset? ScheduleDate { get; set; }

    /// <summary>
    /// For Bitcoin payouts, when this flag is set the network fee will be deducted from the send amount.
    /// This is particularly useful for sweeps where it can be difficult to calculate the exact fee required.
    /// </summary>
    [Obsolete]
    public bool? BitcoinSubtractFeeFromAmount { get; set; }

    /// <summary>
    /// The Bitcoin fee rate to apply in Satoshis per virtual byte.
    /// </summary>
    [Obsolete]
    public int? BitcoinFeeSatsPerVbyte { get; set; }
    
    /// <summary>
    /// Optional. The ID of the beneficiary identifier to use for the payout destination.
    /// </summary>
    [Obsolete("Please use Destination.BeneficiaryID to update the beneficiary.")]
    public Guid? BeneficiaryIdentifierID { get; set; }

    /// <summary>
    /// Optional. The ID of the beneficiary to use for the payout destination.
    /// </summary>
    [Obsolete("Please use Destination.BeneficiaryID to update the beneficiary.")]
    public Guid? BeneficiaryID
    {
        get => Destination?.BeneficiaryID;
        set
        {
            Destination ??= new Counterparty();
            Destination.BeneficiaryID = value;
        }
    }

    /// <summary>
    /// Optional field to indicate the payment rail to use for the payout. Currrently only
    /// supports choosing between SEPA-CT and SEPA-INST for EUR payments. If not set, for a EUR
    /// payment, the default behaviour is to attempt SEPA-INST and fallback to SEPA-CT if rejected.
    /// </summary>
    public PaymentRailEnum? PaymentRail { get; set; }

    /// <summary>
    /// Optional field to set who should pay any fees for the payout. Typically only
    /// used for international payments and ignored for SEPA and Faster Payments.
    /// </summary>
    public PayoutChargeBearerEnum? ChargeBearer { get; set; }

    /// <summary>
    /// Optional. For an FX payout this is the currency that the beneficiary should be sent.
    /// </summary>
    public CurrencyTypeEnum? FxDestinationCurrency { get; set; }

    /// <summary>
    /// Optional but one of Amount or FxDestinationAmount must be set. If specified this will be the amount sent to the payee.
    /// The payout's Amount will be dynamically adjusted based on this amount and the FX rate.
    /// </summary>
    [Range(1.00, double.MaxValue, ErrorMessage = "Minimum value of 1.00 is required for FxDestinationAmount.")]
    public decimal? FxDestinationAmount { get; set; }

    /// <summary>
    /// For a multi-currency payout this indicates how the Amount and FxDestinaationAmount are treated.
    /// If true the FxDestinationAmount is authoritative and the Amount is set based on the FxRate. If false then the Amount is authoritative
    /// and the FxDestinationAmount is set based on the Amount and FxRate.
    /// </summary>
    public bool? FxUseDestinationAmount { get; set; }

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
        if (PaymentRail != null) dict.Add(nameof(PaymentRail), PaymentRail.Value.ToString());
        if (ChargeBearer != null) dict.Add(nameof(ChargeBearer), ChargeBearer.Value.ToString());
        if (FxDestinationCurrency != null) dict.Add(nameof(FxDestinationCurrency), FxDestinationCurrency.Value.ToString());
        if (FxDestinationAmount != null) dict.Add(nameof(FxDestinationAmount), FxDestinationAmount.Value.ToString());
        if (FxUseDestinationAmount != null) dict.Add(nameof(FxUseDestinationAmount), FxUseDestinationAmount.Value.ToString());

        if (Destination != null)
        {
            dict = dict.Concat(Destination.ToDictionary($"{nameof(Destination)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }
}