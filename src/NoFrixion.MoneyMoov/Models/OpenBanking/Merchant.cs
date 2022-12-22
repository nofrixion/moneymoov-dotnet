
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Merchant")]
public partial class Merchant
{
    [DefaultValue("")]
    [DataMember(Name = "merchantName", EmitDefaultValue = false)]
    public string MerchantName { get; set; } = string.Empty;

    [DefaultValue("")]
    [DataMember(Name = "merchantCategoryCode", EmitDefaultValue = false)]
    public string MerchantCategoryCode { get; set; } = string.Empty;
}
