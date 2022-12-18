
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "AccountBalance")]
public partial class AccountBalance
{
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public AccountBalanceType? Type { get; set; }

    [DataMember(Name = "dateTime", EmitDefaultValue = false)]
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets or Sets BalanceAmount
    /// </summary>  
    [DataMember(Name = "balanceAmount", EmitDefaultValue = false)]
    public Amount BalanceAmount { get; set; } = new Amount();

    /// <summary>
    /// _Optional_. Indicates whether any credit lines are included in the balance.
    /// </summary>
    /// <value>_Optional_. Indicates whether any credit lines are included in the balance.</value>
    [DataMember(Name = "creditLineIncluded", EmitDefaultValue = true)]
    public bool CreditLineIncluded { get; set; }

    /// <summary>
    /// _Optional_. Specifies the type of balance.
    /// </summary>
    /// <value>_Optional_. Specifies the type of balance.</value>
    [DataMember(Name = "creditLines", EmitDefaultValue = false)]
    public List<CreditLine> CreditLines { get; set; } = new List<CreditLine>();

    /// <summary>
    /// Returns the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class AccountBalance {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  DateTime: ").Append(DateTime).Append("\n");
        sb.Append("  BalanceAmount: ").Append(BalanceAmount).Append("\n");
        sb.Append("  CreditLineIncluded: ").Append(CreditLineIncluded).Append("\n");
        sb.Append("  CreditLines: ").Append(CreditLines).Append("\n");
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
