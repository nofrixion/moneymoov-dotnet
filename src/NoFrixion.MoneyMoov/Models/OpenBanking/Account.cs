
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

[DataContract(Name = "Account")]
public partial class Account
{
    [DataMember(Name = "usageType", EmitDefaultValue = false)]
    public UsageType? UsageType { get; set; }

    /// <summary>
    /// The type of account e.g. (Credit Card, Savings).
    /// </summary>
    [DataMember(Name = "accountType", EmitDefaultValue = false)]
    public AccountType? AccountType { get; set; }

    /// <summary>
    /// Unique identifier of the account.
    /// </summary>
    [DataMember(Name = "id", EmitDefaultValue = true)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Specifies the type of account e.g. (BUSINESS_CURRENT).
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "type", EmitDefaultValue = false)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Product name as defined by the financial institution for this account.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "description", EmitDefaultValue = false)]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Main or headline balance for the account. Use of this field is recommended as fallback only. Instead, use of the typed balances (accountBalances) is recommended.
    /// </summary>
    [DataMember(Name = "balance", EmitDefaultValue = false)]
    public decimal Balance { get; set; }

    /// <summary>
    /// Currency the bank account balance is denoted in. Specified as a 3-letter ISO 4217 currency code.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "currency", EmitDefaultValue = false)]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Nickname of the account that was provided by the account owner. May be used to aid identification of the account.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "nickname", EmitDefaultValue = false)]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// Supplementary specifications that might be provided by the Bank. These provide further characteristics about the account.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "details", EmitDefaultValue = false)]
    public string Details { get; set; } = string.Empty;

    /// <summary>
    /// Optional account names set by the account holder.
    /// </summary>
    [DataMember(Name = "accountNames", EmitDefaultValue = false)]
    public List<AccountName>? AccountNames { get; set; }

    /// <summary>
    /// The canoncial identifiers for the account.
    /// </summary>
    [DataMember(Name = "accountIdentifications", EmitDefaultValue = false)]
    public List<AccountIdentification>? AccountIdentifications { get; set; }

    /// <summary>
    /// The various balances for the account.
    /// </summary>
    [DataMember(Name = "accountBalances", EmitDefaultValue = false)]
    public List<AccountBalance>? AccountBalances { get; set; }

    /// <summary>
    /// Summary information regarding account balances of the overall account provided by the bank.
    /// </summary>
    [DataMember(Name = "consolidatedAccountInformation", EmitDefaultValue = false)]
    public ConsolidatedAccountInformation? ConsolidatedAccountInformation { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class Account {\n");
        sb.Append("  Id: ").Append(Id).Append("\n");
        sb.Append("  Type: ").Append(Type).Append("\n");
        sb.Append("  Description: ").Append(Description).Append("\n");
        sb.Append("  Balance: ").Append(Balance).Append("\n");
        sb.Append("  Currency: ").Append(Currency).Append("\n");
        sb.Append("  UsageType: ").Append(UsageType).Append("\n");
        sb.Append("  AccountType: ").Append(AccountType).Append("\n");
        sb.Append("  Nickname: ").Append(Nickname).Append("\n");
        sb.Append("  Details: ").Append(Details).Append("\n");
        sb.Append("  AccountNames: ").Append(AccountNames).Append("\n");
        sb.Append("  AccountIdentifications: ").Append(AccountIdentifications).Append("\n");
        sb.Append("  AccountBalances: ").Append(AccountBalances).Append("\n");
        sb.Append("  ConsolidatedAccountInformation: ").Append(ConsolidatedAccountInformation).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
    {
        return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
    }
}
