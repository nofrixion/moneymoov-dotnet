
using System.Runtime.Serialization;
using System.Text;


namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// __Mandatory__. The &#x60;Payee&#x60; object contains details of the beneficiary [person or business]. You must define this in your payment request alongwith all of the nested mandatory properties.
/// </summary>
[DataContract(Name = "Payee")]
public partial class Payee
{
    /// <summary>
    /// __Mandatory__. The account holder name of the beneficiary.
    /// </summary>
    /// <value>__Mandatory__. The account holder name of the beneficiary.</value>
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// __Mandatory__. The account identifications that identify the &#x60;Payee&#x60; bank account.
    /// </summary>
    /// <value>__Mandatory__. The account identifications that identify the &#x60;Payee&#x60; bank account.</value>
    [DataMember(Name = "accountIdentifications", IsRequired = true, EmitDefaultValue = true)]
    public List<AccountIdentification> AccountIdentifications { get; set; } = new List<AccountIdentification>();

    /// <summary>
    /// Gets or Sets Address
    /// </summary>
    [DataMember(Name = "address", EmitDefaultValue = false)]
    public Address Address { get; set; } = new Address();

    /// <summary>
    /// __Optional__. The merchant ID is a unique code provided by the payment processor to the merchant.
    /// </summary>
    /// <value>__Optional__. The merchant ID is a unique code provided by the payment processor to the merchant.</value>
    [DataMember(Name = "merchantId", EmitDefaultValue = false)]
    public string MerchantId { get; set; } = string.Empty;

    /// <summary>
    /// __Optional__. The category code of the merchant in case the &#x60;Payee&#x60; is a business.
    /// </summary>
    /// <value>__Optional__. The category code of the merchant in case the &#x60;Payee&#x60; is a business.</value>
    [DataMember(Name = "merchantCategoryCode", EmitDefaultValue = false)]
    public string MerchantCategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
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

    /// <summary>
    /// Returns the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
