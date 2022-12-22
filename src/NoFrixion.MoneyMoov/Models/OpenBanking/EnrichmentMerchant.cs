
using System.Runtime.Serialization;
using System.ComponentModel;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "EnrichmentMerchant")]
public partial class EnrichmentMerchant
{
    [DefaultValue("")]
    [DataMember(Name = "merchantName", EmitDefaultValue = false)]
    public string MerchantName { get; set; } = string.Empty;

    [DefaultValue("")]
    [DataMember(Name = "parentGroup", EmitDefaultValue = false)]
    public string ParentGroup { get; set; } = string.Empty;
}
