
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "ProprietaryBankTransactionCode")]
public partial class ProprietaryBankTransactionCode
{
    [DefaultValue("")]
    [DataMember(Name = "code", EmitDefaultValue = false)]
    public string Code { get; set; } = string.Empty;

    [DefaultValue("")]
    [DataMember(Name = "issuer", EmitDefaultValue = false)]
    public string Issuer { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ProprietaryBankTransactionCode {\n");
        sb.Append("  Code: ").Append(Code).Append("\n");
        sb.Append("  Issuer: ").Append(Issuer).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
