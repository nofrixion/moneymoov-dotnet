
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// EnrichmentMerchant
/// </summary>
[DataContract(Name = "EnrichmentMerchant")]
public partial class EnrichmentMerchant
{
    [DataMember(Name = "merchantName", EmitDefaultValue = false)]
    public string MerchantName { get; set; } = string.Empty;

    [DataMember(Name = "parentGroup", EmitDefaultValue = false)]
    public string ParentGroup { get; set; } = string.Empty;
}
