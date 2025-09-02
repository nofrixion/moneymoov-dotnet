//-----------------------------------------------------------------------------
// Filename: Counterparty.cs
//
// Description: The counterparty in a transaction. For a pay in (credit) the
// counterparty is the sender of the payment. For a pay out (debit) the 
// counterparty is the destination for the payment.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Jan 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 20 May 2024  Aaron Clauson   Added BeneficiaryID.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class Counterparty
{
    /// <summary>
    /// An optional ID of an internal account the counterparty is associated with. If set
    /// it will take precedence over any other destination details set for the counterparty.
    /// </summary>
    public Guid? AccountID { get; set; }
    
    /// <summary>
    /// If the counterparty is an internal account, this is the name of the account.
    /// </summary>
    public string? InternalAccountName { get; set; } = string.Empty;

    /// <summary>
    /// Optional ID of a Beneficiary to use for the counterparty destination. If set
    /// it will take precedence over any other destination details, except for AccountID,
    /// set for the counterparty.
    /// </summary>
    public Guid? BeneficiaryID { get; set; }

    /// <summary>
    /// The name of the counterparty. For a person this should be their full name. For a 
    /// company this should be their registered or trading name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// An email address for the counterparty. Optional to set and depending on the payment
    /// network does not always get set for pay ins.
    /// </summary>
    public string? EmailAddress { get; set; }

    /// <summary>
    /// A phone number for the counterparty. Optional to set and depending on the payment
    /// network does not always get set for pay ins.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// A country code for the counterparty. Optional to set and depending on the payment
    /// network does not always get set for pay ins
    /// </summary>
    public string? CountryCode { get; set; }

    /// <summary>
    /// The counterparty's account identifier. This identifier is what is used to send the payment
    /// to them, or for a pay in is the source of the payment.
    /// </summary>
    public AccountIdentifier? Identifier { get; set; }
    
    /// <summary>
    /// The type of counter party - Individual or Business.
    /// </summary>
    public CounterpartyType Type { get; set; }

    /// <summary>
    /// Gets a convenient summary representation of the counterparty.
    /// </summary>
    public string Summary
        => Name + (Identifier != null ? ", " + Identifier.Summary : string.Empty);

    public bool IsAccountIDSet() => AccountID != null && AccountID != Guid.Empty;

    public bool IsBeneficiaryIDSet() => BeneficiaryID != null && BeneficiaryID != Guid.Empty;

    public bool IsIdentifierSet() => Identifier != null && (
        !string.IsNullOrEmpty(Identifier.IBAN) ||
        (!string.IsNullOrEmpty(Identifier.AccountNumber) && !string.IsNullOrEmpty(Identifier.SortCode)));

    public int DestinationsSetCount() =>
        (IsAccountIDSet() ? 1 : 0) + (IsBeneficiaryIDSet() ? 1 : 0) + (IsIdentifierSet() ? 1 : 0);

    public bool IsSameDestination(Counterparty? other)
    {
        if (other == null)
        {
            return false;
        }

        if (IsAccountIDSet() && other.IsAccountIDSet())
        {
            return AccountID == other.AccountID;
        }

        if (IsBeneficiaryIDSet() && other.IsBeneficiaryIDSet())
        {
            return BeneficiaryID == other.BeneficiaryID;
        }

        if (IsIdentifierSet() && other.IsIdentifierSet())
        {
            return Identifier!.IsSameDestination(other.Identifier);
        }

        return false;
    }   

    public virtual Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = new Dictionary<string, string>
        {
             { keyPrefix + nameof(AccountID), AccountID != null ? AccountID.Value.ToString() : string.Empty},
             { keyPrefix + nameof(BeneficiaryID), BeneficiaryID != null ? BeneficiaryID.Value.ToString() : string.Empty},
             { keyPrefix + nameof(Name), Name ?? string.Empty },
             { keyPrefix + nameof(EmailAddress), EmailAddress ?? string.Empty },
             { keyPrefix + nameof(PhoneNumber), PhoneNumber ?? string.Empty },
             { keyPrefix + nameof(CountryCode), CountryCode ?? string.Empty },
        };

        if(Identifier != null)
        {
            var identifierDict = Identifier.ToDictionary(keyPrefix + nameof(Identifier) + ".");

            dict = dict.Concat(identifierDict)
               .ToLookup(x => x.Key, x => x.Value)
               .ToDictionary(x => x.Key, g => g.First());
        }

        return dict;
    }

    public virtual string GetApprovalHash()
    {
        string input =
            (AccountID != null && AccountID != Guid.Empty ? AccountID.ToString() : string.Empty) +
            (BeneficiaryID != null && BeneficiaryID != Guid.Empty ? BeneficiaryID.ToString() : string.Empty) +
            (!string.IsNullOrEmpty(Name) ? Name : string.Empty) +
            (!string.IsNullOrEmpty(EmailAddress) ? EmailAddress : string.Empty) +
            (!string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : string.Empty) +
            (!string.IsNullOrEmpty(CountryCode) ? CountryCode : string.Empty) +
            (Identifier != null ? Identifier.GetApprovalHash() : string.Empty);
        return HashHelper.CreateHash(input);
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DestinationsSetCount() == 0)
        {
            yield return new ValidationResult($"One of the destination options (AccountID, BeneficiaryID or Identifier) must be set for a counterparty.",
                null);
        }

        if (IsIdentifierSet() && Identifier?.Type == AccountIdentifierType.Unknown)
        {
            yield return new ValidationResult($"The counterparty identifier must have either an IBAN or account number and sort code set.",
                null);
        }
    }

    public override string ToString()
    {
        return $"Identifier: {Identifier}";
    }
}
