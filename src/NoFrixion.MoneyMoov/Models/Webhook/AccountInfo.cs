using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models
{
    public class AccountInfo
    {
        [JsonProperty("account_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? AccountId { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("identifier")]
        public Identifier? Identifier { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public IdentifierType Type
        {
            get
            {
                if (!string.IsNullOrEmpty(Iban))
                {
                    return IdentifierType.IBAN;
                }

                if (!string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(Number))
                {
                    return IdentifierType.SCAN;
                }

                // Return default
                return IdentifierType.NONE;
            }
        }

        // Only available for SCAN type
        [JsonProperty("number", NullValueHandling = NullValueHandling.Ignore)]
        public string? Number { get; set; }

        // Only available for SCAN type
        [JsonProperty("SortCode", NullValueHandling = NullValueHandling.Ignore)]
        public string? SortCode { get; set; }

        // Only available for IBAN type
        [JsonProperty("iban", NullValueHandling = NullValueHandling.Ignore)]
        public string? Iban { get; set; }
    }

    public enum IdentifierType
    {
        [EnumMember(Value = "NONE")]
        NONE,

        [EnumMember(Value = "IBAN")]
        IBAN,

        [EnumMember(Value = "SCAN")]
        SCAN
    }
}