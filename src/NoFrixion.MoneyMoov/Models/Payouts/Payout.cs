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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class Payout : IValidatableObject, IWebhookPayload
{
    /// <summary>
    /// The ID for the payout.
    /// </summary>
    public Guid ID { get; set; }

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
    /// Gets or Sets your reference ID
    /// </summary>
    public string? YourReference { get; set; }

    /// <summary>
    /// Gets or Sets destination reference ID
    /// </summary>
    public string? TheirReference { get; set; }

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
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
    public string? DestinationIBAN
    {
        get => Destination?.Identifier?.IBAN;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier();
            Destination.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? DestinationAccountNumber
    {
        get => Destination?.Identifier?.AccountNumber;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier();
            Destination.Identifier.AccountNumber = value;
        }
    }

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? DestinationSortCode
    {
        get => Destination?.Identifier?.SortCode;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier();
            Destination.Identifier.SortCode = value;
        }
    }

    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
    public string? DestinationAccountName
    {
        get => Destination?.Name;
        set
        {
            Destination ??= new Counterparty();
            Destination.Name = value;
        }
    }

    public string MerchantTokenDescription { get; set; } = string.Empty;

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

    /// <summary>
    /// The name of the account the payout is being made from.
    /// </summary>
    public string SourceAccountName { get; set; } = string.Empty;

    /// <summary>
    /// The IBAN of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountIban { get; set; } = string.Empty;
    
    /// <summary>
    /// The account number of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountNumber { get; set; } = string.Empty;
    
    /// <summary>
    /// The sort code of the account the payout is being made from.
    /// </summary>
    public string? SourceAccountSortcode { get; set; } = string.Empty;

    /// <summary>
    /// The available balance of the account the payout is being made from.
    /// </summary>
    public decimal? SourceAccountAvailableBalance { get; set; }
    
    [Obsolete("Please use Destination.")]
    [System.Text.Json.Serialization.JsonIgnore]
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
    public string InvoiceID { get; set; } = string.Empty;

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
                AccountID.ToString() +
                Currency +
                Math.Round(Amount, 2).ToString() +
                Destination.GetApprovalHash() +
                Status.ToString() +
                Scheduled.GetValueOrDefault().ToString() +
                ScheduleDate?.ToString("o");

            return HashHelper.CreateHash(input);
        }
    }
}
