
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "ProprietaryBankTransactionCode")]
public partial class ProprietaryBankTransactionCode
{
    /// <summary>
    /// Gets or Sets Code
    /// </summary>
    [DataMember(Name = "code", EmitDefaultValue = false)]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets Issuer
    /// </summary>
    [DataMember(Name = "issuer", EmitDefaultValue = false)]
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ProprietaryBankTransactionCode {\n");
        sb.Append("  Code: ").Append(Code).Append("\n");
        sb.Append("  Issuer: ").Append(Issuer).Append("\n");
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
