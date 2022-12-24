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
//  License: MIT
// -----------------------------------------------------------------------------

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// The payment webhook.
/// </summary>
public class PaymentWebHook
{
    /// <summary>
    /// Gets or sets the event.
    /// </summary>
    [JsonProperty("event")]
    [JsonConverter(typeof(StringEnumConverter))]
    public WebhookEventTypesEum Event { get; set; }

    /// <summary>
    /// Gets or sets the data.
    /// </summary>
    [JsonProperty("data")]
    public PayInfo Data { get; set; } = PayInfo.Empty;

    /// <summary>
    /// Places payment related properties into a dictionary used to send email
    /// notification from the webhook.
    /// </summary>
    /// <returns>A dictionary with payment details properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToEmailDictionary()
    {
        var dict = new Dictionary<string, string>();

        var fromAccountIndentifier = Data.From.Identifier?.Iban
            ?? Data.From.Identifier?.SortCode
            ?? Data.From.Identifier?.Number
            ?? string.Empty;

        var toAccountIndentifier = Data.To.Identifier?.Iban
            ?? Data.To.Identifier?.SortCode
            ?? Data.To.Identifier?.Number
            ?? string.Empty;

        dict.Add("PaymentID", Data.Id?.ToString().ToUpper() ?? string.Empty);
        dict.Add("PaymentType", Event.ToString());
        dict.Add("Amount", Data.Amount.ToString("F"));
        dict.Add("Description", Data.Description);
        dict.Add("Date", Data.Date.ToString("dd MMM yyyy HH:mm:ss"));
        dict.Add("FromAccountName", Data.From.Name ?? string.Empty);
        dict.Add("FromAccountType", $"{Data.From.Identifier?.Type} - {fromAccountIndentifier}");
        dict.Add("ToAccountName", Data.To.Name ?? string.Empty);
        dict.Add("ToAccountType", $"{Data.From.Identifier?.Type} - {toAccountIndentifier}");

        return dict;
    }
}