
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// Summary information regarding account balances of the overall account provided by the bank.
/// </summary>
[DataContract(Name = "ConsolidatedAccountInformation")]
public partial class ConsolidatedAccountInformation
{
    /// <summary>
    /// Identifier of the consolidated account. When used in Get Account Transactions calls, the transactions 
    /// between the sub-accounts will not be reported.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// The various balances for the account.
    /// </summary>
    [DataMember(Name = "accountBalances", EmitDefaultValue = false)]
    public List<AccountBalance>? AccountBalances { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class ConsolidatedAccountInformation {\n");
        sb.Append("  Id: ").Append(Id).Append("\n");
        sb.Append("  AccountBalances: ").Append(AccountBalances).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
