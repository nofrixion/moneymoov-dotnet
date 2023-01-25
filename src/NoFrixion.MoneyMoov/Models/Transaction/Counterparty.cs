﻿//-----------------------------------------------------------------------------
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
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class Counterparty
{
    public Guid? AccountID { get; set; }

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

    /// An email address for the counterparty. Optional to set and depending on the payment
    /// network does not always get set for pay ins.
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// The counterparty's account identifier. This identifier is what is used to send the payment
    /// to them, or for a pay in is the source of the payment.
    /// </summary>
    public AccountIdentifier? Identifier { get; set; }

    /// <summary>
    /// Gets a convenient summary representation of the counterparty.
    /// </summary>
    public string Summary
        => Name + (Identifier != null ? ", " + Identifier.Summary : string.Empty);

    public virtual Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        var dict = new Dictionary<string, string>
        {
             { keyPrefix + nameof(AccountID), AccountID != null ? AccountID.Value.ToString() : string.Empty},
             { keyPrefix + nameof(Name), Name ?? string.Empty },
             { keyPrefix + nameof(EmailAddress), EmailAddress ?? string.Empty },
             { keyPrefix + nameof(PhoneNumber), PhoneNumber ?? string.Empty },
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
}
