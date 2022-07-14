#nullable disable

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models
{
    public class Webhook
    {
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookEvent Event { get; set; }

        [JsonProperty("data")]
        public PayInfo Data { get; set; }
    }

    public enum WebhookEvent
    {
        [EnumMember(Value = "PAY_IN")]
        PAYIN,

        [EnumMember(Value = "PAY_OUT")]
        PAYOUT
    }
}
