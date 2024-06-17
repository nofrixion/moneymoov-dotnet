
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Payer")]
public partial class Payer
{
    /// <summary>
    /// The account holder name of the Payer.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The account identifications that identify the Payer; bank account.
    /// </summary>
    [DataMember(Name = "accountIdentifications", IsRequired = true, EmitDefaultValue = true)]
    public List<AccountIdentification>? AccountIdentifications { get; set; }

    [DataMember(Name = "address", EmitDefaultValue = false)]
    public Address? Address { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Payer {\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
        sb.Append("  AccountIdentifications: ").Append(AccountIdentifications).Append("\n");
        sb.Append("  Address: ").Append(Address).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
