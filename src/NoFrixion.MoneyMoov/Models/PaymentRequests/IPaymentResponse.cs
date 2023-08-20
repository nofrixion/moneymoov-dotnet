// -----------------------------------------------------------------------------
//  Filename: IPaymentResponse.cs
// 
//  Description: Interface for payment responses generated after processing payments.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  29 11 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public interface IPaymentResponse
{
    public Guid PaymentRequestID { get; set; }

    public PaymentResponseType ResponseType { get; }
}