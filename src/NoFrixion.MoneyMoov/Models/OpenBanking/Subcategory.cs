
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Subcategory")]
public partial class Subcategory
{
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public Guid Id { get; set; }

    [DataMember(Name = "label", EmitDefaultValue = false)]
    public string Label { get; set; } = string.Empty;
}
