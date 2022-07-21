using Newtonsoft.Json;
using System.Runtime.Serialization;

#nullable disable

namespace NoFrixion.MoneyMoov.Models
{
    public class Transaction
    {
        /// <summary>
        /// Transaction id which always starts with 'T'.
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// Id of the Payout
        /// <para>Only available for transaction type <see cref="TransactionType.PAYOUT"/></para>
        /// </summary>
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }

        /// <summary>
        /// Type of the transaction
        /// <para>Available options are <see cref="TransactionType.PAYOUT"/> and <see cref="TransactionType.PAYIN"/></para>
        /// </summary>
        [JsonProperty("type")]
        public TransactionType Type { get; set; }

        /// <summary>
        /// Amount transfered
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Currency of amount
        /// </summary>
        [JsonProperty("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Description of the transaction
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Date when the transaction was created (in UTC)
        /// </summary>
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Date when the transaction happened (in UTC)
        /// </summary>
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Account of who sent the transaction
        /// </summary>
        [JsonProperty("from")]
        public AccountInfo From { get; set; }

        /// <summary>
        /// Account of who receive the transaction
        /// </summary>
        [JsonProperty("to")]
        public AccountInfo To { get; set; }
    }

    public enum TransactionType
    {
        [EnumMember(Value = "NONE")]
        NONE,

        [EnumMember(Value = "PAY_IN")]
        PAYIN,

        [EnumMember(Value = "PAY_OUT")]
        PAYOUT
    }
}
