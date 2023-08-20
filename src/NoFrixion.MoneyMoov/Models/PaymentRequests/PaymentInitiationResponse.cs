// -----------------------------------------------------------------------------
//  Filename: PaymentInitiationResponse.cs
// 
//  Description: Bank Payment Response:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  26 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//  27 Jan 2022 Aaron Clauson    Renamed from BankPaymentResponse to
//                               PaymentInitiationResponse.
// 
//  License:
//  MIT
// -----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class PaymentInitiationResponse : IPaymentResponse
{
    /// <summary>
    /// The unique identifier of the payment initiation request.
    /// </summary>
    [DataMember(Name = "paymentInitiationID", EmitDefaultValue = false)]
    public string PaymentInitiationID { get; set; }

    /// <summary>
    /// A redirect URL for the user to authorise the payment initiation request at the ASPSP
    /// </summary>
    [DataMember(Name = "redirectUrl", EmitDefaultValue = false)]
    public string RedirectUrl { get; set; }

    [DataMember(Name = "specificErrorMessage", EmitDefaultValue = false)]
    public string SpecificErrorMessage { get; set; }

    /// <summary>
    /// The callback URL that was set when the payment request was created. Payers will be 
    /// redirected to this URL after a successful payment initiation.
    /// </summary>
    public string PaymentRequestCallbackUrl { get; set; }

    public Guid PaymentRequestID { get; set; }

    public PaymentResponseType ResponseType => PaymentResponseType.PaymentInitiationResponse;
}