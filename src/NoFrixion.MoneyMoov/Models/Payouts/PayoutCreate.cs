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
    /// <summary>
    /// The ID of the source account for the payout.
    /// </summary>
    [Required(ErrorMessage = "AccountID is required.")]
    public Guid AccountID { get; set; }

    /// <summary>
    /// The type of account identifier to use for the payout destination.
    /// </summary>
    /// <example>IBAN</example>
    [Required(ErrorMessage = "The type of payout to create is required.")]
    public AccountIdentifierType Type { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Optional but one of Amount or FxDestinationAmount must be set. 
    /// - For single currency payouts this property is mandatory. 
    /// - For mulit-currency payouts this property is optional. If FxDestinationAmount is set this field must be set to 0
    /// and it will be dynamically adjusted based on the FX rate.
    /// </summary>
    [Range(0.01, double.MaxValue,ErrorMessage = "Minimum value of 0.01 is required for Amount.")]
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
    /// For Bitcoin payouts, when this flag is set the network fee will be deducted from the send amount. This is particularly useful for sweeps where it can be difficult to calculate the exact fee required.
    /// </summary>
    [Obsolete]
    public bool BitcoinSubtractFeeFromAmount { get; set; }

    /// <summary>
    /// The Bitcoin fee rate to apply in Satoshis per virtual byte.
    /// </summary>
    [Obsolete]
    public int BitcoinFeeSatsPerVbyte { get; set; }
    
    /// <summary>
    /// Optional. The ID of the beneficiary identifier to use for the payout destination.
    /// </summary>
    [Obsolete("Please use Destination.BeneficiaryID to set the beneficiary.")]
    public Guid? BeneficiaryIdentifierID { get; set; }

    /// <summary>
    /// Optional. The ID of the beneficiary to use for the payout destination.
    /// </summary>
    //[Obsolete("Please use Destination.BeneficiaryID to set the beneficiary.")]
    public Guid? BeneficiaryID
    {
        get => Destination?.BeneficiaryID;
        set
        {
            Destination ??= new CounterpartyCreate();
            Destination.BeneficiaryID = value;
        }
    }

    /// <summary>
    /// The ID of the batch payout this payout is part of.
    /// </summary>
    public Guid? BatchPayoutID { get; set; }

    /// <summary>
    /// Optional, if set it indicates that this payout will be used to top up 
    /// a payment account for a pay run by an internal transfer.
    /// </summary>
    public Guid? TopupPayrunID { get; set; }

    /// <summary>
    /// Optional field to indicate the payment rail to use for the payout. Currrently only
    /// supports choosing between SEPA-CT and SEPA-INST for EUR payments. If not set, for a EUR
    /// payment, the default behaviour is to attempt SEPA-INST and fallback to SEPA-CT if rejected.
    /// </summary>
    public PaymentRailEnum PaymentRail { get; set; }
    
    /// <summary>
    /// List of documents to attach to the payout. Optional.
    /// Used for identifying or associating documents with the payout.
    /// </summary>
    public List<PayoutDocumentCreate>? Documents { get; set; }

    /// <summary>
    /// Optional field to set who should pay any fees for the payout. Typically only
    /// used for international payments and ignored for SEPA and Faster Payments.
    /// </summary>
    public PayoutChargeBearerEnum ChargeBearer { get; set; }

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
            { nameof(PaymentRail), PaymentRail.ToString() },
            { nameof(ChargeBearer), ChargeBearer.ToString() }
        };

        if (Destination != null)
        {
            dict = dict.Concat(Destination.ToDictionary($"{nameof(Destination)}."))
                .ToLookup(x => x.Key, x => x.Value)
                .ToDictionary(x => x.Key, g => g.First());
        }

        if (FxDestinationCurrency != null) dict.Add(nameof(FxDestinationCurrency), FxDestinationCurrency.Value.ToString());
        if (FxDestinationAmount != null) dict.Add(nameof(FxDestinationAmount), FxDestinationAmount.Value.ToString());

        return dict;
    }
}
