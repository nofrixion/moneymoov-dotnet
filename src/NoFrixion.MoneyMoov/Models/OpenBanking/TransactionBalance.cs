
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

    [DataMember(Name = "balanceAmount", EmitDefaultValue = false)]
    public Amount? BalanceAmount { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TransactionBalance {\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  BalanceAmount: ").Append(BalanceAmount).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
