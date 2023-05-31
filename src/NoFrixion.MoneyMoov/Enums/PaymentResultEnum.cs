// -----------------------------------------------------------------------------
//  Filename: PaymentResultEnum.cs
// 
//  Description: Enum for Status of payment request
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 08 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum PaymentResultEnum
{
    /// <summary>
    /// No events have been recorded for the payment request.
    /// </summary>
    None,

    /// <summary>
    /// At least one event has been recorded for the payment request
    /// but it has not yet been finalised.
    /// </summary>
    //InProgress,

    /// <summary>
    /// The payment attempt was successful and the request amount has been fully paid.
    /// </summary>
    FullyPaid,

    /// <summary>
    /// The payment attempt was successful but the amount paid was less than the requested amount.
    /// </summary>
    PartiallyPaid,

    /// <summary>
    /// Payments over the expected amount have been received.
    /// </summary>
    OverPaid,

    /// <summary>
    /// For card payments the payment was voided prior to settlement.
    /// </summary>
    Voided,

    /// <summary>
    /// For PISP payments, the authorization was successful but the amount is yet to be settled in the respective account.
    /// </summary>
    Authorized,

    /// <summary>
    /// For PISP payments, when a refund is initiated for a payment request.
    /// </summary>
    RefundInitiated
}