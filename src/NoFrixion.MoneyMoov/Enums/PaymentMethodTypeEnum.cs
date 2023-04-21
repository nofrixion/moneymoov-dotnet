// -----------------------------------------------------------------------------
//  Filename: PaymentMethodTypeEnum.cs
// 
//  Description: Type enum for payment methods:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  01 Dec 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

[Flags]
public enum PaymentMethodTypeEnum
{
    /// <summary>
    /// No payment methods.
    /// </summary>
    None = 0,

    /// <summary>
    /// Credit and debit cards.
    /// </summary>
    card = 1,

    /// <summary>
    /// Payment initiation payment, also known as open banking account-to-account.
    /// </summary>
    pisp = 2,

    /// <summary>
    /// Bitcoin Lightning Network.
    /// </summary>
    lightning = 4,

    /// <summary>
    /// Pay with a previously stored card token.
    /// </summary>
    cardtoken = 8,

    /// <summary>
    /// Pay with Apple Pay
    /// </summary>
    applePay = 16,

    /// <summary>
    /// Pay with Google Pay
    /// </summary>
    googlePay = 32
}