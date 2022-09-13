// -----------------------------------------------------------------------------
//  Filename: PaymentRequestSuccessWebHook.cs
// 
//  Description: The payment request success webhook class.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  12 09 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License: MIT.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace NoFrixion.MoneyMoov.Models;


/// <summary>
/// The payment request success web hook.
/// </summary>
public class PaymentRequestSuccessWebHook : WebHook
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentRequestSuccessWebHook"/> class.
    /// </summary>
    public PaymentRequestSuccessWebHook()
    {
        this.Event = WebhookEvent.PAYMENTREQUESTSUCCESS;
    }

    /// <summary>
    /// Payment request ID.
    /// </summary>
    [JsonProperty("paymentRequestID")]
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// Success web hook Url.
    /// </summary>
    [JsonProperty("successWebHookUrl")]
    public string SuccessWebHookUrl { get; set; } = string.Empty;

    /// <summary>
    /// Payment request order ID.
    /// </summary>
    [JsonProperty("orderID")]
    public string OrderID { get; set; } = string.Empty;
}