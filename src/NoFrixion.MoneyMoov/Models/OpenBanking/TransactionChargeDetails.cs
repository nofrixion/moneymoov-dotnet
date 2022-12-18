
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// TransactionChargeDetails
/// </summary>
[DataContract(Name = "TransactionChargeDetails")]
public partial class TransactionChargeDetails
{
    /// <summary>
    /// Gets or Sets ChargeAmount
    /// </summary>
    [DataMember(Name = "chargeAmount", EmitDefaultValue = false)]
    public Amount ChargeAmount { get; set; } = new Amount();

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TransactionChargeDetails {\n");
        sb.Append("  ChargeAmount: ").Append(ChargeAmount).Append("\n");
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
