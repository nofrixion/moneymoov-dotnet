
using System.Runtime.Serialization;
using System.ComponentModel;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Subcategory")]
public partial class Subcategory
{
    [DefaultValue("")]
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public Guid Id { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "label", EmitDefaultValue = false)]
    public string Label { get; set; } = string.Empty;
}
