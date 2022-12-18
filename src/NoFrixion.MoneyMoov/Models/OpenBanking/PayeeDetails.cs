
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "PayeeDetails")]
public partial class PayeeDetails
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
    /// __Conditional__. The 2-letter country code for the address. &lt;br&gt;&lt;br&gt;An &#x60;Institution&#x60; may require you to specify the &#x60;country&#x60; when used in the context of the &#x60;Payee&#x60; to be able to make a payment
    /// </summary>
    /// <value>__Conditional__. The 2-letter country code for the address. &lt;br&gt;&lt;br&gt;An &#x60;Institution&#x60; may require you to specify the &#x60;country&#x60; when used in the context of the &#x60;Payee&#x60; to be able to make a payment</value>
    [DataMember(Name = "country", IsRequired = true, EmitDefaultValue = true)]
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class PayeeDetails {\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
        sb.Append("  AccountIdentifications: ").Append(AccountIdentifications).Append("\n");
        sb.Append("  Country: ").Append(Country).Append("\n");
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
