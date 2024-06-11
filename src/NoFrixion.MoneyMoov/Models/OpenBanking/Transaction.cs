

using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Transaction")]
public partial class Transaction
{
    [DataMember(Name = "status", EmitDefaultValue = false)]
    public TransactionStatusEnum? Status { get; set; }

    [DefaultValue("")]
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

    [DefaultValue("")]
    [DataMember(Name = "currency", EmitDefaultValue = false)]
    public string Currency { get; set; } = string.Empty;

    [DataMember(Name = "transactionAmount", EmitDefaultValue = false)]
    public Amount? TransactionAmount { get; set; }

    [DataMember(Name = "grossAmount", EmitDefaultValue = false)]
    public Amount? GrossAmount { get; set; }

    [DataMember(Name = "currencyExchange", EmitDefaultValue = false)]
    public CurrencyExchange? CurrencyExchange { get; set; }

    [DataMember(Name = "chargeDetails", EmitDefaultValue = false)]
    public TransactionChargeDetails? ChargeDetails { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "reference", EmitDefaultValue = false)]
    public string Reference { get; set; } = string.Empty;

    [DataMember(Name = "statementReferences", EmitDefaultValue = false)]
    public List<StatementReference>? StatementReferences { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "description", EmitDefaultValue = false)]
    public string Description { get; set; } = string.Empty;

    [DataMember(Name = "transactionInformation", EmitDefaultValue = false)]
    public List<string>? TransactionInformation { get; set; }

    [DataMember(Name = "addressDetails", EmitDefaultValue = false)]
    public AddressDetails? AddressDetails { get; set; }

    [DataMember(Name = "isoBankTransactionCode", EmitDefaultValue = false)]
    public IsoBankTransactionCode? IsoBankTransactionCode { get; set; }

    [DataMember(Name = "proprietaryBankTransactionCode", EmitDefaultValue = false)]
    public ProprietaryBankTransactionCode? ProprietaryBankTransactionCode { get; set; }

    [DataMember(Name = "balance", EmitDefaultValue = false)]
    public TransactionBalance? Balance { get; set; }

    [DataMember(Name = "payeeDetails", EmitDefaultValue = false)]
    public Payee? PayeeDetails { get; set; }

    [DataMember(Name = "payerDetails", EmitDefaultValue = false)]
    public Payer? PayerDetails { get; set; } 

    [DataMember(Name = "merchant", EmitDefaultValue = false)]
    public Merchant? Merchant { get; set; }

    [DataMember(Name = "enrichment", EmitDefaultValue = false)]
    public Enrichment? Enrichment { get; set; }

    [DataMember(Name = "supplementaryData", EmitDefaultValue = false)]
    public Object? SupplementaryData { get; set; }

    [DefaultValue("")]
    [DataMember(Name = "transactionMutability", EmitDefaultValue = false)]
    public string TransactionMutability { get; set; } = string.Empty;

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Transaction {\n");
        sb.Append("  Id: ").Append(Id).Append("\n");
        sb.Append("  Date: ").Append(Date).Append("\n");
        sb.Append("  BookingDateTime: ").Append(BookingDateTime).Append("\n");
        sb.Append("  ValueDateTime: ").Append(ValueDateTime).Append("\n");
        sb.Append("  Status: ").Append(Status).Append("\n");
        sb.Append("  Amount: ").Append(Amount).Append("\n");
        sb.Append("  Currency: ").Append(Currency).Append("\n");
        sb.Append("  TransactionAmount: ").Append(TransactionAmount).Append("\n");
        sb.Append("  GrossAmount: ").Append(GrossAmount).Append("\n");
        sb.Append("  CurrencyExchange: ").Append(CurrencyExchange).Append("\n");
        sb.Append("  ChargeDetails: ").Append(ChargeDetails).Append("\n");
        sb.Append("  Reference: ").Append(Reference).Append("\n");
        sb.Append("  StatementReferences: ").Append(StatementReferences).Append("\n");
        sb.Append("  Description: ").Append(Description).Append("\n");
        sb.Append("  TransactionInformation: ").Append(TransactionInformation).Append("\n");
        sb.Append("  AddressDetails: ").Append(AddressDetails).Append("\n");
        sb.Append("  IsoBankTransactionCode: ").Append(IsoBankTransactionCode).Append("\n");
        sb.Append("  ProprietaryBankTransactionCode: ").Append(ProprietaryBankTransactionCode).Append("\n");
        sb.Append("  Balance: ").Append(Balance).Append("\n");
        sb.Append("  PayeeDetails: ").Append(PayeeDetails).Append("\n");
        sb.Append("  PayerDetails: ").Append(PayerDetails).Append("\n");
        sb.Append("  Merchant: ").Append(Merchant).Append("\n");
        sb.Append("  Enrichment: ").Append(Enrichment).Append("\n");
        sb.Append("  SupplementaryData: ").Append(SupplementaryData).Append("\n");
        sb.Append("  TransactionMutability: ").Append(TransactionMutability).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
