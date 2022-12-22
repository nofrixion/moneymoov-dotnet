
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "PayeeDetails")]
public partial class PayeeDetails
{
    /// <summary>
    /// The account holder name of the beneficiary.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "name", IsRequired = true, EmitDefaultValue = true)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The account identifications that identify the Payee bank account.
    /// </summary>
    [DataMember(Name = "accountIdentifications", IsRequired = true, EmitDefaultValue = true)]
    public List<AccountIdentification>? AccountIdentifications { get; set; }

    /// <summary>
    /// The 2-letter country code for the address. Institutions may require you to specify the country when used in the context of the Payee to be able to make a payment.
    /// </summary>
    [DataMember(Name = "country", IsRequired = true, EmitDefaultValue = true)]
    public string Country { get; set; } = string.Empty;

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

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
