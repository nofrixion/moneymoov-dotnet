
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AccountIdentification")]
public partial class AccountIdentification
{
    [DataMember(Name = "type", IsRequired = true, EmitDefaultValue = true)]
    public AccountIdentificationType Type { get; set; }

    /// <summary>
    /// The value associated with the account identification type.
    /// </summary>
    [DataMember(Name = "identification", IsRequired = true, EmitDefaultValue = true)]
    public string Identification { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AccountIdentification {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  Identification: ").Append(Identification).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}