
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "TransactionHash")]
public partial class TransactionHash
{
    [DataMember(Name = "hash", EmitDefaultValue = false)]
    public string Hash { get; set; } = string.Empty;
}
