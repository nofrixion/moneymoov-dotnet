//-----------------------------------------------------------------------------
// Filename: CardCustomerToken.cs
//
// Description: Represents the public details of a card that has been tokenised
// on behalf of a merchant's customer.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// Anzac Day 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class CardCustomerToken
{
    /// <summary>
    /// The unique ID of the card token that has been stored for the customer. This is 
    /// the ID to supply when requesting an authorisation on behalf of the customer.
    /// </summary>
    public Guid ID { get; set; }

    public string MaskedCardNumber { get; set; } = string.Empty;

    public string LastFourDigits { get; set; } = string.Empty;

    public string ExpiryMonth { get; set; } = string.Empty;

    public string ExpiryYear { get; set; } = string.Empty;

    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// When creating a tokenised card the payer's email address must be supplied. This is
    /// used as away to group card tokens for an end user. For customer initiated transactions 
    /// it is important that the email address supplied has been verified to belong to the user
    /// initiating the payment.
    /// </summary>
    [EmailAddress]
    public string CustomerEmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// The type of the tokenised card, e.g. Visa, MasterCard etc. It's possible this could
    /// be empty if the card type could not be identified. There is no hard and fast way to
    /// know for sure to know the type of card.
    /// </summary>
    public string CardType { get; set; } = string.Empty;

    public Guid MerchantID { get; set; }

    public DateTimeOffset Inserted { get; set; }
    
    public DateTimeOffset LastUpdated { get; set; }
}
