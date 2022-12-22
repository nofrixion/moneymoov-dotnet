
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "IsoBankTransactionCode")]
public partial class IsoBankTransactionCode
{
    [DataMember(Name = "domainCode", EmitDefaultValue = false)]
    public IsoCodeDetails? DomainCode { get; set; }

    [DataMember(Name = "familyCode", EmitDefaultValue = false)]
    public IsoCodeDetails? FamilyCode { get; set; }

    [DataMember(Name = "subFamilyCode", EmitDefaultValue = false)]
    public IsoCodeDetails? SubFamilyCode { get; set; }
}
