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
// MIT.
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

    /// <summary>
    /// A payment initiation attempt was initiated. A webhook is sent out
    /// when the status of the payment changes.
    /// </summary>
    pisp_webhook = 11,

    /// <summary>
    /// A successful PISP attempt was verified and settled through modulr transaction.
    /// </summary>
    pisp_settle = 12,

    /// <summary>
    /// A PISP attempt was authorised but then funds failed to settle after an expiry
    /// period (typically 2 business days).
    /// </summary>
    pisp_settle_failure = 13,

    /// <summary>
    /// A PIS payment is refund is initated to user. A PIS refund event can only be called when 
    /// the payment request status is fully paid.  
    /// </summary>
    pisp_refund_initiated = 14,

    /// <summary>
    /// A PIS payment is refund is complete. The funds have been transferred back to user.
    /// </summary>
    pisp_refund_settled = 15,
    
    /// <summary>
    /// A PIS payment is refund is cancelled. The funds have not been transferred back to user.
    /// </summary>
    pisp_refund_cancelled = 16,

    /// <summary>
    /// A card payment was initiated. A webhook is sent out when the status of the payment changes. 
    /// </summary>
    card_webhook = 17,
    
    /// <summary>
    /// A card payment was refunded.
    /// </summary>
    card_refund = 18,
}