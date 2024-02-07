//-----------------------------------------------------------------------------
// Filename: CardPaymetnResponseStatus.cs
//
// Description: List of the response statuses returned by card payment processors.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Nov 2022 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class CardPaymentResponseStatus
{
    public const string PAYMENT_AUTHORIZED_STATUS = "PAYMENT_AUTHORIZED";

    /// <summary>
    /// The successful status result returned for an authorisation or sale card payment request.
    /// </summary>
    public const string CARD_AUTHORIZED_SUCCESS_STATUS = "AUTHORIZED";

    /// <summary>
    /// Status response that indicates the payer needs to be redirected to
    /// their issuing bank to complete a 3-D Secure payer authentication step.
    /// </summary>
    public const string PENDING_AUTHENTICATION_STATUS = "PENDING_AUTHENTICATION";

    /// <summary>
    /// A successful capture request will received a status of pending to indicate the payment
    /// has been submitted for settlement.
    /// </summary>
    public const string CARD_CAPTURE_SUCCESS_STATUS = "PENDING";

    /// <summary>
    /// Status response that indicates the payer authentication setup completed successfully.
    /// </summary>
    public const string CARD_PAYER_AUTHENTICATION_SETUP_COMPLETE = "COMPLETED";

    /// <summary>
    /// A soft decline is a special error response to a card authorisation. It indicates
    /// the authorisation was successful but that an address, card verification number,
    /// or other secondary validation check failed. The authorisation needs to be explicitly 
    /// captured.
    /// </summary>
    public const string CARD_PAYMENT_SOFT_DECLINE_STATUS = "202";

    /// <summary>
    /// The successful status result returned for a void card payment request.
    /// </summary>
    public const string CARD_VOIDED_SUCCESS_STATUS = "VOIDED";

    /// <summary>
    /// The successful status result returned for a CyberSource void card authorisation.
    /// </summary>
    public const string CARD_CYBERSOURCE_AUTHORISATION_VOIDED_SUCCESS_STATUS = "REVERSED";

    /// <summary>
    /// The successful status result returned for a CyberSource void card capture or sale.
    /// </summary>
    public const string CARD_CYBERSOURCE_CAPTURE_VOIDED_SUCCESS_STATUS = "VOIDED";

    /// <summary>
    /// The successful status for a checkout.com captured card payment.
    /// </summary>
    public const string CARD_CHECKOUT_CAPTURED_STATUS = "CAPTURED";

    /// <summary>
    /// The successful status result returned for a checkout authorisation card payment request.
    /// </summary>
    public const string CARD_CHECKOUT_AUTHORIZED_STATUS = "Authorized";

    /// <summary>
    /// The successful status result returned for a checkout authorisation card payment request.
    /// </summary>
    public const string CARD_CHECKOUT_CARDVERFIED_STATUS = "CardVerified";
    
    /// <summary>
    /// The successful status result returned for a cybersource refund card payment request.
    /// </summary>
    public const string CARD_CYBERSOURCE_REFUNDED_SUCCESS_STATUS = "PENDING";
    
    /// <summary>
    /// The successful status result returned for a refund card payment request.
    /// </summary>
    public const string CARD_CHECKOUT_REFUNDED_SUCCESS_STATUS = "REFUNDED";
}
