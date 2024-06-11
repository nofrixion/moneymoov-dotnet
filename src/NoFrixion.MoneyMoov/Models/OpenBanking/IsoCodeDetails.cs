
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "IsoCodeDetails")]
public partial class IsoCodeDetails
{
    [DefaultValue("")]
    [DataMember(Name = "code", EmitDefaultValue = false)]
    public string Code { get; set; } = "UNKNOWN";

    [DefaultValue("")]
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; } = "UNKNOWN";

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class IsoCodeDetails {\n");
        sb.Append("  Code: ").Append(Code).Append("\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
