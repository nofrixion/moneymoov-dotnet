
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Category")]
public partial class Category
{
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public Guid Id { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "label", EmitDefaultValue = false)]
    public string Label { get; set; } = string.Empty;

    [DefaultValue("")]
    [DataMember(Name = "country", EmitDefaultValue = false)]
    public string Country { get; set; } = string.Empty;

    [DataMember(Name = "subcategories", EmitDefaultValue = false)]
    public List<Subcategory>? Subcategories { get; set; }
}
