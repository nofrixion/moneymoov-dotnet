// -----------------------------------------------------------------------------
// Filename: PaymentRequestEventTypesEnum.cs
// 
// Description: Enum for the different types of payment request events that can
// occur.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 14 Dec 2021 Aaron Clauson    Created, Stillorgan Wood, Dublin, Ireland.
// 30 Dec 2021 Aaron Clauson    Renamed from PaymentReceiveStatusEnum to PaymentRequestEventTypesEnum.
// 
// License:
// Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum PaymentRequestEventTypesEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    unknown = 0,

    /// <summary>
    /// A request was made to set up payer authentication as part of a card payment.
    /// </summary>
    card_payer_authentication_setup = 1,

    /// <summary>
    /// A request was made to authorise a payment using a card. An authorization does not
    /// capture (initiate settlement) and therefore does not count towards funds paid to
    /// the merchant.
    /// </summary>
    card_authorization = 2,

    /// <summary>
    /// A request was made to authorise and capture a payment using a card. A card sale
    /// does initiate settlement and does count towards funds paid to a merchant.
    /// </summary>
    card_sale = 3,

    /// <summary>
    /// A request was made to capture (settle) a payment after a prior card authorisation step.
    /// </summary>
    card_capture = 4,

    /// <summary>
    /// A request was made to void a card authorisation, capture or sale (authorisation + capture).
    /// </summary>
    card_void = 5,

    /// <summary>
    /// A payment initiation attempt was initiated. The initiation will generate a redirect URL for the 
    /// payer. There is no guarantee they will complete the authorisation.
    /// </summary>
    pisp_initiate = 6,

    /// <summary>
    /// A callback received after a payment initiation attempt. Typically the callback occurs after
    /// the payer has completed or explicitly cancelled the payment attempt on their issuing bank's
    /// web site.
    /// </summary>
    pisp_callback = 7,

    /// <summary>
    /// A Bitcoin Lightning invoice was generated for the payer.
    /// </summary>
    lightning_invoice_created = 8,

    /// <summary>
    /// A Bitcoin Lightning invoice was paid.
    /// </summary>
    lightning_invoice_paid = 9,

    /// <summary>
    /// A payer authentication failure callback for a card payment.
    /// </summary>
    card_payer_authentication_failure = 10,
}