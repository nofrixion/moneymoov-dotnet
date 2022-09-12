// -----------------------------------------------------------------------------
//  Filename: PaymentWebHook.cs
// 
//  Description: The payment webhook class with PAYIN or PAYOUT events.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  12 09 2022  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using Newtonsoft.Json;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// The payment webhook.
/// </summary>
public class PaymentWebHook : WebHook
{
    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    [JsonProperty("data")]
    public PayInfo Data { get; set; }
}