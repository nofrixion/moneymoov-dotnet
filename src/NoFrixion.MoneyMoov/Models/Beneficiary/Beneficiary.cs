// -----------------------------------------------------------------------------
//  Filename: Beneficiary.cs
// 
//  Description: Class containing information for a beneficiary:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  11 05 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

#nullable disable

namespace NoFrixion.MoneyMoov.Models;

public class Beneficiary : IValidatableObject
{
    public Guid ID { get; set; }

    /// <summary>
    /// Gets or Sets the merchant id.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// The descriptive name for the beneficiary.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets the currency.
    /// </summary>
    [Required(ErrorMessage = "Currency is required.")]
    public CurrencyTypeEnum Currency { get; set; }

    public Counterparty Destination { get; set; }

    /// <summary>
    /// The ID of the beneficiary identifier.
    /// </summary>
    [Obsolete("No longer used.")]
    public Guid BeneficiaryIdentifierID { get; set; }
    
    public string ApprovalCallbackUrl { get; set; }

    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// A list of users who have successfully authorised the latest version of the beneficiary.
    /// </summary>
    [CanBeNull] public List<UserMinimal> AuthorisedBy { get; set; }

    /// <summary>
    /// True if the beneficiary can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the beneficiary can be updated by the user who loaded it.
    /// </summary>
    public bool CanUpdate { get; set; }

    /// <summary>
    /// True if the beneficiary was loaded for a user and that user has already authorised the latest version of the beneficiary.
    /// </summary>
    public bool HasCurrentUserAuthorised {  get; set; }
    
    /// <summary>
    /// The number of authorisers required for this beneficiary. Is determined by business settings
    /// on the source account and/or merchant.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the beneficiary.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }
    
    public string CreatedByEmailAddress { get; set; }
    
    public string Nonce { get; set; }
    
    public DateTimeOffset Inserted { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
    
    public DateTimeOffset? LastAuthorised { get; set; }
    
    public User CreatedBy { get; set; }

    // Don't serialize the events if there are none.
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [Newtonsoft.Json.JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual IEnumerable<BeneficiaryEvent> BeneficiaryEvents { get; set; }

    public virtual IEnumerable<PaymentAccount> SourceAccounts { get; set; }

    public bool ShouldSerializeBeneficiaryEvents()
    {
        return BeneficiaryEvents != null && BeneficiaryEvents.Any();
    }

    /// <summary>
    /// Gets a hash of the critical fields for the beneficiary. This hash is
    /// used to ensure a beneficiary's details are not modified between the time the
    /// authorisation is given and the time the beneficiary is enabled.
    /// </summary>
    /// <returns>A hash of the beneficiary's critical fields.</returns>
    public string GetApprovalHash()
    {
        var input =
            Name + 
            MerchantID + 
            GetSourceAccountsHash() +
            Currency +
            Destination.GetApprovalHash()
            + (string.IsNullOrEmpty(Nonce) ? string.Empty : Nonce);

        return HashHelper.CreateHash(input);
    }

    /// <summary>
    /// Gets a hash of the source account IDs authorised for the beneficiary.
    /// If no source accounts are associated it means the beneficiary is authorised for all
    /// the merchant's accounts.
    /// </summary>
    /// <returns>A hash of the source accounts or an empty string if the beneficiary is authorised
    /// for all the merchant's accounts.</returns>
    public string GetSourceAccountsHash()
        => SourceAccounts?.Count() > 0
                ? HashHelper.CreateHash(string.Join(',', SourceAccounts.Select(x => x.ID.ToString())))
                : string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var payout = ToPayout();
        payout.Amount = 0.01M;
        // Use dummy valid values for references. Beneficiaries can have invalid references as
        // they will be forced to fix them when the payout is created.
        payout.TheirReference = "GoodRef";
        payout.YourReference = "GoodRef";
        return payout.Validate(validationContext);
    }

    public NoFrixionProblem Validate()
    {
        var context = new ValidationContext(this, serviceProvider: null, items: null);

        List<ValidationResult> validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(this, context, validationResults, true);

        // Apply biz validation rules.
        var bizValidationResults = Validate(context);
        if (bizValidationResults.Count() > 0)
        {
            isValid = false;
            validationResults.AddRange(bizValidationResults);
        }

        return isValid ?
            NoFrixionProblem.Empty :
            new NoFrixionProblem($"The {nameof(Beneficiary)} had one or more validation errors.", validationResults);
    }

    /// <summary>
    /// Maps the beneficiary to a Payout. Used for creating a new Payout and also validating the 
    /// Beneficiary.
    /// </summary>
    /// <returns>A new Payout object.</returns>
    public Payout ToPayout()
    {
        var payout = new Payout
        {
            ID = Guid.NewGuid(),
            Type = Destination?.Identifier?.Type ?? AccountIdentifierType.Unknown,
            Currency = Currency,
            Destination = Destination,
        };

        if (payout.Destination?.Identifier != null)
        {
            payout.Destination.Identifier.Currency = Currency;
        }

        return payout;
    }
}