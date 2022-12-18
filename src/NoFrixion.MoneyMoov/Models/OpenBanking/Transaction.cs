

using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// Transaction
/// </summary>
[DataContract(Name = "Transaction")]
public partial class Transaction
{
    [DataMember(Name = "status", EmitDefaultValue = false)]
    public TransactionStatusEnum? Status { get; set; }

    [DataMember(Name = "id", EmitDefaultValue = false)]
    public string Id { get; set; } = string.Empty;

    [DataMember(Name = "date", EmitDefaultValue = false)]
    public DateTime Date { get; set; }

    [DataMember(Name = "bookingDateTime", EmitDefaultValue = false)]
    public DateTime BookingDateTime { get; set; }

    [DataMember(Name = "valueDateTime", EmitDefaultValue = false)]
    public DateTime ValueDateTime { get; set; }

    [DataMember(Name = "amount", EmitDefaultValue = false)]
    public decimal Amount { get; set; }

    [DataMember(Name = "currency", EmitDefaultValue = false)]
    public string Currency { get; set; } = string.Empty;

    [DataMember(Name = "transactionAmount", EmitDefaultValue = false)]
    public Amount TransactionAmount { get; set; } = new Amount();

    [DataMember(Name = "grossAmount", EmitDefaultValue = false)]
    public Amount GrossAmount { get; set; } = new Amount();

    [DataMember(Name = "currencyExchange", EmitDefaultValue = false)]
    public CurrencyExchange CurrencyExchange { get; set; } = new CurrencyExchange();

    /// <summary>
    /// Gets or Sets ChargeDetails
    /// </summary>
    [DataMember(Name = "chargeDetails", EmitDefaultValue = false)]
    public TransactionChargeDetails ChargeDetails { get; set; } = new TransactionChargeDetails();

    /// <summary>
    /// Gets or Sets Reference
    /// </summary>
    [DataMember(Name = "reference", EmitDefaultValue = false)]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets StatementReferences
    /// </summary>
    [DataMember(Name = "statementReferences", EmitDefaultValue = false)]
    public List<StatementReference> StatementReferences { get; set; } = new List<StatementReference>();

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name = "description", EmitDefaultValue = false)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or Sets TransactionInformation
    /// </summary>
    [DataMember(Name = "transactionInformation", EmitDefaultValue = false)]
    public List<string> TransactionInformation { get; set; } = new List<string>();

    /// <summary>
    /// Gets or Sets AddressDetails
    /// </summary>
    [DataMember(Name = "addressDetails", EmitDefaultValue = false)]
    public AddressDetails AddressDetails { get; set; } = new AddressDetails();

    /// <summary>
    /// Gets or Sets IsoBankTransactionCode
    /// </summary>
    [DataMember(Name = "isoBankTransactionCode", EmitDefaultValue = false)]
    public IsoBankTransactionCode IsoBankTransactionCode { get; set; } = new IsoBankTransactionCode();

    /// <summary>
    /// Gets or Sets ProprietaryBankTransactionCode
    /// </summary>
    [DataMember(Name = "proprietaryBankTransactionCode", EmitDefaultValue = false)]
    public ProprietaryBankTransactionCode ProprietaryBankTransactionCode { get; set; } = new ProprietaryBankTransactionCode();

    /// <summary>
    /// Gets or Sets Balance
    /// </summary>
    [DataMember(Name = "balance", EmitDefaultValue = false)]
    public TransactionBalance Balance { get; set; } = new TransactionBalance();

    /// <summary>
    /// Gets or Sets PayeeDetails
    /// </summary>
    [DataMember(Name = "payeeDetails", EmitDefaultValue = false)]
    public Payee PayeeDetails { get; set; } = new Payee();

    /// <summary>
    /// Gets or Sets PayerDetails
    /// </summary>
    [DataMember(Name = "payerDetails", EmitDefaultValue = false)]
    public Payer PayerDetails { get; set; } = new Payer();

    [DataMember(Name = "merchant", EmitDefaultValue = false)]
    public Merchant Merchant { get; set; } = new Merchant();

    [DataMember(Name = "enrichment", EmitDefaultValue = false)]
    public Enrichment Enrichment { get; set; } = new Enrichment();

    [DataMember(Name = "supplementaryData", EmitDefaultValue = false)]
    public Object SupplementaryData { get; set; } = new object();

    [DataMember(Name = "transactionMutability", EmitDefaultValue = false)]
    public string TransactionMutability { get; set; } = string.Empty;
}
