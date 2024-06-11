
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

/// <summary>
/// Provides details on the currency exchange.
/// </summary>
[DataContract(Name = "CurrencyExchange")]
public class CurrencyExchange
{
    /// <summary>
    /// Currency from which an amount is to be converted.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "sourceCurrency", EmitDefaultValue = false)]
    public string SourceCurrency { get; set; } = string.Empty;

    /// <summary>
    /// Currency to which an amount is to be converted.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "targetCurrency", EmitDefaultValue = false)]
    public string TargetCurrency { get; set; } = string.Empty;

    /// <summary>
    /// The currency in which the rate of exchange is expressed in a currency exchange. In the example 1GBP the unit currency is GBP.
    /// </summary>
    [DefaultValue("")]
    [DataMember(Name = "unitCurrency", EmitDefaultValue = false)]
    public string UnitCurrency { get; set; } = string.Empty;

    /// <summary>
    /// The factor used for conversion of an amount from one currency to another. This reflects the price at which one currency was 
    /// bought with another currency.
    /// </summary>
    [DataMember(Name = "exchangeRate", EmitDefaultValue = false)]
    public decimal ExchangeRate { get; set; }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("class CurrencyExchange {\n");
        sb.Append("  SourceCurrency: ").Append(SourceCurrency).Append("\n");
        sb.Append("  TargetCurrency: ").Append(TargetCurrency).Append("\n");
        sb.Append("  UnitCurrency: ").Append(UnitCurrency).Append("\n");
        sb.Append("  ExchangeRate: ").Append(ExchangeRate).Append("\n");
        sb.Append("}\n");
        return sb.ToString();
    }

    public virtual string ToJson()
        => this.ToJsonFormatted();
}
