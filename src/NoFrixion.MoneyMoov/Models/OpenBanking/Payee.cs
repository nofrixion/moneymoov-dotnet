
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// The Payee object contains details of the beneficiary, person or business.
/// </summary>
[DataContract(Name = "Payee")]
public partial class Payee
{
    /// <summary>
    /// The account holder name of the beneficiary.
    /// </summary>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The account identifications that identify the Payee  bank account.
    /// </summary>
    [DataMember(Name = "accountIdentifications", IsRequired = true, EmitDefaultValue = false)]
    public List<AccountIdentification>? AccountIdentifications { get; set; }

    [DataMember(Name = "address", EmitDefaultValue = false)]
    public Address? Address { get; set; }

    /// <summary>
    /// The merchant ID is a unique code provided by the payment processor to the merchant.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "merchantId", EmitDefaultValue = false)]
    public string MerchantId { get; set; } = string.Empty;

    /// <summary>
    /// The category code of the merchant in case the Payee is a business.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "merchantCategoryCode", EmitDefaultValue = false)]
    public string MerchantCategoryCode { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Payee {\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
        sb.Append("  AccountIdentifications: ").Append(AccountIdentifications).Append("\n");
        sb.Append("  Address: ").Append(Address).Append("\n");
        sb.Append("  MerchantId: ").Append(MerchantId).Append("\n");
        sb.Append("  MerchantCategoryCode: ").Append(MerchantCategoryCode).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
