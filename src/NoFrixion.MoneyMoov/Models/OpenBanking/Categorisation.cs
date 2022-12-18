

using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Categorisation")]
public partial class Categorisation
{
    [DataMember(Name = "categories", EmitDefaultValue = false)]
    public List<string> Categories { get; set; } = new List<string>();

    [DataMember(Name = "source", EmitDefaultValue = false)]
    public string Source { get; set; } = string.Empty;
}
