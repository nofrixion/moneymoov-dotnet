
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "StatementReference")]
public partial class StatementReference
{
    [DefaultValue("")]
    [DataMember(Name = "value", EmitDefaultValue = false)]
    public string Value { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class StatementReference {\n");
        sb.Append("  Value: ").Append(Value).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
