// -----------------------------------------------------------------------------
//  Filename: PaymentRequestStatusFilterEnum.cs
// 
//  Description: Enum to filter payment requests on Status
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  03 08 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License: MIT
// -----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace NoFrixion.MoneyMoov;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PaymentRequestStatusFilterEnum
{
    /// <summary>
    /// Default filter to get all payment requests regardless of Status
    /// </summary>
    All,

    /// <summary>
    /// No events have been recorded for the payment request.
    /// </summary>
    None,

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
    Authorized
}