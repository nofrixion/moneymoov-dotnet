using Newtonsoft.Json;

#nullable disable

namespace NoFrixion.MoneyMoov.Models
{
    public class PayInfo
    {
        // Only available for PAYOUT
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("from")]
        public AccountInfo From { get; set; }

        [JsonProperty("to")]
        public AccountInfo To { get; set; }
    }
}
