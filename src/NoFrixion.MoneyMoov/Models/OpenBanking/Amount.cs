
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Amount")]
public partial class Amount
{
    /// <summary>
    /// The monetary value.
    /// </summary>
    [DataMember(Name = "amount", IsRequired = true, EmitDefaultValue = true)]
    public decimal _Amount { get; set; }

    /// <summary>
    /// The [ISO 4217](https://www.xe.com/iso4217.php) currency code.
    /// </summary>
    [DataMember(Name = "currency", IsRequired = true, EmitDefaultValue = true)]
    public string Currency { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Amount {\n");
        sb.Append("  _Amount: ").Append(_Amount).Append("\n");
        sb.Append("  Currency: ").Append(Currency).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
