
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// TransactionBalance
/// </summary>
[DataContract(Name = "TransactionBalance")]
public partial class TransactionBalance
{
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public AccountBalanceType? Type { get; set; }

    /// <summary>
    /// Gets or Sets BalanceAmount
    /// </summary>
    [DataMember(Name = "balanceAmount", EmitDefaultValue = false)]
    public Amount BalanceAmount { get; set; } = new Amount();

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TransactionBalance {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  BalanceAmount: ").Append(BalanceAmount).Append("\n");
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
