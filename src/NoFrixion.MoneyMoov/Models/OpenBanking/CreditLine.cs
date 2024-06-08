

using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "CreditLine")]
public partial class CreditLine
{
    /// <summary>
    /// Type of the credit line.
    /// </summary>
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public CreditLineType? Type { get; set; }

    /// <summary>
    /// The amount of the credit line.
    /// </summary>
    [DataMember(Name = "creditLineAmount", EmitDefaultValue = false)]
    public Amount? CreditLineAmount { get; set; } 

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CreditLine {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  CreditLineAmount: ").Append(CreditLineAmount).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
