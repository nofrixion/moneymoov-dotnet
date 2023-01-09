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
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class Counterparty
{
    public Guid? AccountId { get; set; }

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
}
