
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AddressDetails")]
public partial class AddressDetails
{
    /// <summary>
    /// Information, in free format text, that identifies a specific address.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "addressLine", EmitDefaultValue = false)]
    public string AddressLine { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AddressDetails {\n");
        sb.Append("  AddressLine: ").Append(AddressLine).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
