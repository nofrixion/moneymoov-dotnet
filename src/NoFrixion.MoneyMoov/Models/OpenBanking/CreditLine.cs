

using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// CreditLine
/// </summary>
[DataContract(Name = "CreditLine")]
public partial class CreditLine
{
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public CreditLineType? Type { get; set; }

    /// <summary>
    /// Gets or Sets CreditLineAmount
    /// </summary>
    [DataMember(Name = "creditLineAmount", EmitDefaultValue = false)]
    public Amount CreditLineAmount { get; set; } = new Amount();

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CreditLine {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  CreditLineAmount: ").Append(CreditLineAmount).Append("\n");
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
