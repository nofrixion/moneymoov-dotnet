

using System.Runtime.Serialization;
using System.ComponentModel;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Categorisation")]
public partial class Categorisation
{
    [DataMember(Name = "categories", EmitDefaultValue = false)]
    public List<string>? Categories { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "source", EmitDefaultValue = false)]
    public string Source { get; set; } = string.Empty;
}
