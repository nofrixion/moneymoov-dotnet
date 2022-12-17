#nullable disable

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models
{
    /// <summary>
    /// The webhook event.
    /// </summary>
    public enum WebhookEvent
    {
        /// <summary>
        /// The none.
        /// </summary>
        [EnumMember(Value = "NONE")]
        NONE,

        /// <summary>
        /// The pay in event.
        /// </summary>
        [EnumMember(Value = "PAY_IN")]
        PAYIN,

        /// <summary>
        /// The payout event.
        /// </summary>
        [EnumMember(Value = "PAY_OUT")]
        PAYOUT,
    }

    /// <summary>
    /// The webhook class.
    /// </summary>
    public class WebHook
    {
        /// <summary>
        /// Gets or sets the event.
        /// </summary>
        [JsonProperty("event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WebhookEvent Event { get; set; }
    }  
}
