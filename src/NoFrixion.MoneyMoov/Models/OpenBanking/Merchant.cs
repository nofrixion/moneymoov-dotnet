
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// Merchant
/// </summary>
[DataContract(Name = "Merchant")]
public partial class Merchant
{
    /// <summary>
    /// Gets or Sets MerchantName
    /// </summary>
    [DataMember(Name = "merchantName", EmitDefaultValue = false)]
    public string MerchantName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets MerchantCategoryCode
    /// </summary>
    [DataMember(Name = "merchantCategoryCode", EmitDefaultValue = false)]
    public string MerchantCategoryCode { get; set; } = string.Empty;
}
