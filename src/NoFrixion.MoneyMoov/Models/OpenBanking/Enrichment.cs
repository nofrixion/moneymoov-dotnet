
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Enrichment")]
public partial class Enrichment
{
    [DataMember(Name = "categorisation", EmitDefaultValue = false)]
    public Categorisation? Categorisation { get; set; }

    [DataMember(Name = "transactionHash", EmitDefaultValue = false)]
    public TransactionHash? TransactionHash { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "cleansedDescription", EmitDefaultValue = false)]
    public string CleansedDescription { get; set; } = string.Empty;

    [DataMember(Name = "merchant", EmitDefaultValue = false)]
    public EnrichmentMerchant? Merchant { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "location", EmitDefaultValue = false)]
    public string Location { get; set; } = string.Empty;

    [DefaultValue("")]
    [DataMember(Name = "paymentProcessor", EmitDefaultValue = false)]
    public string PaymentProcessor { get; set; } = string.Empty;

    [DataMember(Name = "correctedDate", EmitDefaultValue = false)]
    public DateTime CorrectedDate { get; set; }   
}
