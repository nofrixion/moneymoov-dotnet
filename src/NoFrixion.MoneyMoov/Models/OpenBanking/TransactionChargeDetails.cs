
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "TransactionChargeDetails")]
public partial class TransactionChargeDetails
{
    [DataMember(Name = "chargeAmount", EmitDefaultValue = false)]
    public Amount? ChargeAmount { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class TransactionChargeDetails {\n");
        sb.Append("  ChargeAmount: ").Append(ChargeAmount).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
