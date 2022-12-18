
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AccountIdentification")]
public partial class AccountIdentification
{
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public AccountIdentificationType Type { get; set; }

    /// <summary>
    /// __Mandatory__. The value associated with the account identification type.&lt;br&gt;&lt;br&gt; See [Account Identification Combinations](https://docs.yapily.com/pages/key-concepts/payments/payment-execution/intro-to-payment-execution/#account-identifications-combinations) for more information on the format of the values.
    /// </summary>
    /// <value>__Mandatory__. The value associated with the account identification type.&lt;br&gt;&lt;br&gt; See [Account Identification Combinations](https://docs.yapily.com/pages/key-concepts/payments/payment-execution/intro-to-payment-execution/#account-identifications-combinations) for more information on the format of the values.</value>
    [DataMember(Name = "identification", IsRequired = true, EmitDefaultValue = true)]
    public string Identification { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AccountIdentification {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  Identification: ").Append(Identification).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
