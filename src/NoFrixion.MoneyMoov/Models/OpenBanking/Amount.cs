
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Amount")]
public partial class Amount
{
    /// <summary>
    /// __Mandatory__. The monetary value
    /// </summary>
    /// <value>__Mandatory__. The monetary value</value>
    [DataMember(Name = "amount", IsRequired = true, EmitDefaultValue = true)]
    public decimal _Amount { get; set; }

    /// <summary>
    /// __Mandatory__. The [ISO 4217](https://www.xe.com/iso4217.php) currency code
    /// </summary>
    /// <value>__Mandatory__. The [ISO 4217](https://www.xe.com/iso4217.php) currency code</value>
    [DataMember(Name = "currency", IsRequired = true, EmitDefaultValue = true)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Amount {\n");
        sb.Append("  _Amount: ").Append(_Amount).Append("\n");
        sb.Append("  Currency: ").Append(Currency).Append("\n");
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
