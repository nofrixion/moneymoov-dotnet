
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Enrichment")]
public partial class Enrichment
{
    /// <summary>
    /// Gets or Sets Categorisation
    /// </summary>
    [DataMember(Name = "categorisation", EmitDefaultValue = false)]
    public Categorisation Categorisation { get; set; } = new Categorisation();

    /// <summary>
    /// Gets or Sets TransactionHash
    /// </summary>
    [DataMember(Name = "transactionHash", EmitDefaultValue = false)]
    public TransactionHash TransactionHash { get; set; } = new TransactionHash();

    /// <summary>
    /// Gets or Sets CleansedDescription
    /// </summary>
    [DataMember(Name = "cleansedDescription", EmitDefaultValue = false)]
    public string CleansedDescription { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets Merchant
    /// </summary>
    [DataMember(Name = "merchant", EmitDefaultValue = false)]
    public EnrichmentMerchant Merchant { get; set; } = new EnrichmentMerchant();

    /// <summary>
    /// Gets or Sets Location
    /// </summary>
    [DataMember(Name = "location", EmitDefaultValue = false)]
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets PaymentProcessor
    /// </summary>
    [DataMember(Name = "paymentProcessor", EmitDefaultValue = false)]
    public string PaymentProcessor { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets CorrectedDate
    /// </summary>
    [DataMember(Name = "correctedDate", EmitDefaultValue = false)]
    public DateTime CorrectedDate { get; set; }   
}
