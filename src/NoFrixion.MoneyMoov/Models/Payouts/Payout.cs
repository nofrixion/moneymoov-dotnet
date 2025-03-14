// -----------------------------------------------------------------------------
//  Filename: Payout.cs
// 
//  Description: Contains details of a payout request with additional fields for the portal:
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  22 Nov 2021  Arif Matin     Created, Carmichael House, Dublin, Ireland.
//  12 Dec 2022  Aaron Clauson  Renamed from PortalPayout to Payout.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Approve;
using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Extensions;

namespace NoFrixion.MoneyMoov.Models;

public class Payout : IValidatableObject, IWebhookPayload, IExportableToCsv
{
    /// <summary>
    /// The ID for the payout.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// The ID of the payrun that this payout is associated with.
    /// </summary>
    public Guid? PayrunID { get; set; }
    
    /// <summary>
    /// Gets or Sets Account Id of sending account
    /// </summary>
    public Guid AccountID { get; set; }

    /// <summary>
    /// The ID of the merchant that owns the account.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Gets or Sets User ID of who created the payout request
    /// </summary>
    public Guid? UserID { get; set; }

    /// <summary>
    /// Gets the User ID of person that approved the payout.
    /// </summary>
    public Guid? ApproverID { get; set; }

    /// <summary>
    /// The ID of a payrun that needs an account top up. 
    /// Payouts can be used to top up payrun accounts.  
    /// </summary>
    public Guid? TopupPayrunID { get; set; }

    /// <summary>
    /// Gets or Sets payout type
    /// </summary>
    public AccountIdentifierType Type { get; set; }

    /// <summary>
    /// Gets or Sets description of payout request
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets Currency of payout request
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Gets or Sets payout amount
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency and formatted amount string.
    /// </summary>
    public string FormattedAmount => PaymentAmount.DisplayCurrencyAndAmount(Currency, Amount);

    /// <summary>
    /// Gets or Sets your reference ID
    /// </summary>
    public string? YourReference { get; set; }

    /// <summary>
    /// Gets or Sets destination reference ID
    /// </summary>
    public string? TheirReference { get; set; }

    /// <summary>
    /// If set to true indicates the payout has been flagged as safe to process after transaction monitoring.
    /// </summary>
    public bool CanProcess { get; set; }

    /// <summary>
    /// The ID of the batch the payout is associated with.
    /// </summary>
    public Guid? BatchPayoutID { get; set; }
    
    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
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
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? DestinationIBAN
    {
        get => Destination?.Identifier?.IBAN;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier{Currency = CurrencyTypeEnum.EUR};
            Destination.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
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
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
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
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public string? DestinationAccountName
    {
        get => Destination?.Name;
        set
        {
            Destination ??= new Counterparty();
            Destination.Name = value;
        }
    }

    public string? MerchantTokenDescription { get; set; }

    /// <summary>
    /// Gets or Sets the status of payout request
    /// </summary>
    public PayoutStatus Status { get; set; }

    /// <summary>
    /// The ID of the user that requested access to the PayOut record. Note
    /// this is NOT necessarily the user that created it. 
    /// </summary>
    public Guid? CurrentUserID { get; set; }

    /// <summary>
    /// The role of the user that requested access to the PayOut record. Note
    /// this is NOT necessarily the user that created it. For example one user
    /// may create the payout and then a different user will load the record to
    /// approve it.
    /// </summary>
    public UserRolesEnum? CurrentUserRole { get; set; }

    /// <summary>
    /// This field is used when returning an payout record to a client. If set it holds the URL
    /// the user needs to visit in order to complete a strong authentication check in order to approve 
    /// the payout.
    /// </summary>
    public string? ApprovePayoutUrl { get; set; }

    public string? CreatedBy { get; set; }

    public string? CreatedByEmailAddress { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// The name of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountName { get; set; }

    /// <summary>
    /// The IBAN of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountIban { get; set; }

    /// <summary>
    /// The account number of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountNumber { get; set; }

    /// <summary>
    /// The sort code of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountSortcode { get; set; }

    /// <summary>
    /// The current Bitcoin address of the account the payout is being made from.
    /// </summary>
    [Obsolete]
    public string? SourceBitcoinAddress { get; set; }

    public AccountIdentifier? SourceAccountIdentifier
    {
        get
        {
            AccountIdentifier? srcAccountIdentifier = null;
            
            if (string.IsNullOrWhiteSpace(SourceAccountIban) && 
                string.IsNullOrWhiteSpace(SourceAccountNumber) && 
                string.IsNullOrEmpty(SourceAccountSortcode))
            {
                return srcAccountIdentifier;
            }
            
            srcAccountIdentifier = new AccountIdentifier
            {
                IBAN = SourceAccountIban,
                SortCode = SourceAccountSortcode,
                AccountNumber = SourceAccountNumber,
                Currency = Currency
            };
            return srcAccountIdentifier;
        }
    }

    /// <summary>
    /// The available balance of the account the payout is being made from.
    /// </summary>
    public decimal? SourceAccountAvailableBalance { get; set; }

    /// <summary>
    /// The available balance of the account the payout is being made from.
    /// </summary>
    public string FormattedSourceAccountAvailableBalance => PaymentAmount.DisplayCurrencyAndAmount(Currency, SourceAccountAvailableBalance ?? decimal.Zero);

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Counterparty? DestinationAccount
    {
        get => Destination;
        set => Destination = value;
    }

    public Counterparty? Destination { get; set; }

    /// <summary>
    /// Optional field to associate the payout with the invoice from an external 
    /// application such as Xero. The InvoiceID needs to be unique for each
    /// account.
    /// </summary>
    public string? InvoiceID { get; set; }

    /// <summary>
    /// An optional list of descriptive tags attached to the payout.
    /// </summary>
    public List<Tag> Tags { get; set; } = new List<Tag>();

    /// <summary>
    /// Should this payout be scheduled for a future date?
    /// </summary>
    public bool? Scheduled { get; set; }

    /// <summary>
    /// The date the payout should be submitted.
    /// </summary>
    public DateTimeOffset? ScheduleDate { get; set; }

    public string FormattedScheduleDayOnly => PaymentSchedule.GetFormattedScheduleDayOnly(ScheduleDate);

    public string FormattedSchedule => PaymentSchedule.GetFormattedSchedule(ScheduleDate);

    /// <summary>
    /// For Bitcoin payouts, when this flag is set the network fee will be deducted from the send amount.
    /// THis is particularly useful for sweeps where it can be difficult to calculate the exact fee required.
    /// </summary>
    public bool BitcoinSubtractFeeFromAmount { get; set; }

    /// <summary>
    /// The Bitcoin fee rate to apply in Satoshis per virtual byte.
    /// </summary>
    public int BitcoinFeeSatsPerVbyte { get; set; }

    public string FormattedBitcoinFee => $"{BitcoinFeeSatsPerVbyte} sats/vbyte" +
        (BitcoinSubtractFeeFromAmount ? " (fee will be subtracted from amount)" : "");

    /// <summary>
    /// The number of authorisers required for this payout. Is determined by business settings
    /// on the source account and/or merchant.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the payout.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }
    
    /// <summary>
    /// True if the payout can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the payout can be updated by the user who loaded it.
    /// </summary>
    public bool CanUpdate { get; set; }

    /// <summary>
    /// True if the payout was loaded for a user and that user has already authorised the latest version of the payout.
    /// </summary>
    public bool HasCurrentUserAuthorised {  get; set; }

    /// <summary>
    /// A list of the email addresses of all the users who have successfully authorised the latest version of the payout.
    /// </summary>
    [Obsolete("Refer to Authorisations instead.")]
    public List<string>? AuthorisedBy { get; set; }

    /// <summary>
    /// A list of the users who have successfully authorised the latest version of the payout and when.
    /// </summary>
    public List<Authorisation>? Authorisations { get; set; }

    /// <summary>
    /// A list of authentication types allowed to authorise the payout.
    /// </summary>
    public List<AuthenticationTypesEnum>? AuthenticationMethods { get; set; }

    /// <summary>
    /// If the payout destination is a beneficiary this will be the ID of it's identifier.
    /// </summary>
    [Obsolete("No longer used.")]
    public Guid? BeneficiaryIdentifierID { get; set; }
    
    /// <summary>
    /// If the payout is using a beneficiary for the destination this is the name of the it.
    /// </summary>
    [Obsolete("Please use Beneificiary.")]
    public string? BeneficiaryName { get; set; }

    /// <summary>
    /// The name of the payrun that this payout is associated with.
    /// </summary>
    public string? PayrunName { get; set; }

    /// <summary>
    /// Optional beneficiary associated with the payout.
    /// </summary>
    public Beneficiary? Beneficiary { get; set; }

    /// <summary>
    /// The usptream payment processor for the payout.
    /// </summary>
    public PaymentProcessorsEnum PaymentProcessor { get; set; }

    /// <summary>
    /// The activity associated with the payout.
    /// </summary>
    public List<PayoutEvent>? Events { get; set; }
    
    /// <summary>
    /// Details of the rule that triggered the payout.
    /// </summary>
    public RuleMinimal? Rule { get; set; }

    /// <summary>
    /// Optional field to indicate the payment rail to use for the payout. Currrently only
    /// supports choosing between SEPA-CT and SEPA-INST for EUR payments. If not set, for a EUR
    /// payment, the default behaviour is to attempt SEPA-INST and fallback to SEPA-CT if rejected.
    /// </summary>
    public PaymentRailEnum PaymentRail { get; set; }

    public string? Nonce { get; set; }
    
    /// <summary>
    /// Collection of payrun invoices associated with the payout.
    /// Will be empty if the payout is not associated with a payrun.
    /// </summary>
    [Obsolete("Please refer to the Documents collection.")]
    public List<PayrunInvoice>? PayrunInvoices { get; set; }
    
    /// <summary>
    /// Documents associated with the payout.
    /// </summary>
    public List<PayoutDocument>? Documents { get; set; }

    /// <summary>
    /// Indicates whether the payout has been submitted for processing. Once submitted the payout
    /// amount is reserved until the payout is marked as failed or settled.
    /// </summary>
    public bool IsSubmitted { get; set; }

    /// <summary>
    /// Set to true if a submitted payout subsequently fails. If a payout fails the amount is
    /// remvoed from the account's reserved balance.
    /// </summary>
    public bool IsFailed { get; set; }

    /// <summary>
    /// Set to true if a payout was successfully processed and the corresponding transaction has been
    /// recorded on the ledger.
    /// </summary>
    public bool IsSettled { get; set; }

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        if (isValid)
        {
            // If the property validations passed, apply the overall business validation rules.
            var bizRulesvalidationResults = PayoutsValidator.Validate(this, context);
            if (bizRulesvalidationResults.Count() > 0)
            {
                isValid = false;
                validationResults ??= new List<ValidationResult>();
                validationResults.AddRange(bizRulesvalidationResults);
            }
        }

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem("The Payout model had one or more validation errors.", validationResults);
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return PayoutsValidator.Validate(this, validationContext);
    }

    /// <summary>
    /// Gets a hash of the critical fields for the payout. This hash is
    /// used to ensure a payout's details are not modified between the time the
    /// approval is given and the time the payout is actioned.
    /// </summary>
    /// <returns>A hash of the payout's critical fields.</returns>
    public string GetApprovalHash()
    {
        if (Destination == null)
        {
            return string.Empty;
        }
        else
        {
            string input =
                ID.ToString() +
                AccountID.ToString() +
                Currency +
                Math.Round(Amount, Currency.GetDecimalPlaces()).ToString() +
                Destination.GetApprovalHash() +
                Scheduled.GetValueOrDefault().ToString() +
                ScheduleDate?.ToString("o") +
                (string.IsNullOrEmpty(Nonce) ? string.Empty : Nonce);

            return HashHelper.CreateHash(input);
        }
    }

    public string CsvHeader() =>
        $"ID,PayrunID,AccountID,MerchantID,CreatedByUserID,ApproverID,TopupPayrunID,Type,Description,Currency,Amount,YourReference,TheirReference,CanProcess,BatchPayoutID,DestinationAccountID,DestinationIBAN,DestinationAccountNumber,DestinationSortCode,DestinationAccountName,MerchantTokenDescription,Status,ExportedByUserID,ExportedByUserRole,AuthoriseUrl,CreatedByUserName,CreatedByEmailAddress,Inserted,LastUpdated,SourceAccountName,SourceAccountIban,SourceAccountNumber,SourceAccountSortcode,SourceAccountAvailableBalance,InvoiceID,Tags,Scheduled,ScheduleDate,AuthorisersRequiredCount,AuthorisersCompletedCount,Authorisations,AuthenticationMethods,BeneficiaryID,PayrunName,PaymentProcessor,RuleID, RuleName,PaymentRail,Nonce,DocumentIDs,IsSubmitted,IsFailed,IsSettled";
    
    public string ToCsvRow()
    {
        return this.ToCsvRowString();
    }
}
