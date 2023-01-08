using LanguageExt.ClassInstances;
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

        [JsonProperty("emailAddress")]
        public string? EmailAddress { get; set; }

        [JsonProperty("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonProperty("identifier")]
        public Identifier? Identifier { get; set; }

        /// <summary>
        /// Gets a convenient representation of the account info.
        /// </summary>
        public string GetDisplayText()
        {
            var displayText = "name: " + Name;

            if (Identifier != null)
            {
                if (!string.IsNullOrEmpty(Identifier.Iban))
                {
                    displayText += " iban: " + Identifier.Iban;
                }
                else if(!string.IsNullOrEmpty(Identifier.Number))
                {
                    displayText += " sortcode: " + Identifier.SortCode + " account: " + Identifier.Number;
                }
            }

            if(!string.IsNullOrEmpty(EmailAddress))
            {
                displayText += " email: " + EmailAddress;
            }

            if (!string.IsNullOrEmpty(PhoneNumber))
            {
                displayText += " phone: " + PhoneNumber;
            }

            return displayText;
        }
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