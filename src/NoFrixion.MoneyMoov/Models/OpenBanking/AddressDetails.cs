
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AddressDetails")]
public partial class AddressDetails
{
    /// <summary>
    /// Information, in free format text, that identifies a specific address.
    /// </summary>
    /// <value>Information, in free format text, that identifies a specific address.</value>
    [DataMember(Name = "addressLine", EmitDefaultValue = false)]
    public string AddressLine { get; set; } = string.Empty;

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AddressDetails {\n");
        sb.Append("  AddressLine: ").Append(AddressLine).Append("\n");
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
