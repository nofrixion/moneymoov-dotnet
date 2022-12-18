
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AccountName")]
public partial class AccountName
{
    /// <summary>
    /// The bank account holder&#39;s name given by the account owner.
    /// </summary>
    /// <value>The bank account holder&#39;s name given by the account owner.</value>
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AccountName {\n");
        sb.Append("  Name: ").Append(Name).Append("\n");
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
