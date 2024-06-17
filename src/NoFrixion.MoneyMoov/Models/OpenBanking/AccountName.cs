
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AccountName")]
public partial class AccountName
{
    /// <summary>
    /// The bank account holder's name given by the account owner.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AccountName {\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
