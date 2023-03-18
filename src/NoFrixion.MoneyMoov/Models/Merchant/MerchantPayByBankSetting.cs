// -----------------------------------------------------------------------------
//  Filename: MerchantPayByBankSetting.cs
// 
//  Description: Model representing an individual bank payment setting.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  08 03 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Represents an individual bank payment setting.
/// </summary>
public class MerchantPayByBankSetting
{
    /// <summary>
    /// ID of the bank to be configured for the merchant.
    /// </summary>
    public Guid BankID { get; set; }

    /// <summary>
    /// Name of the Bank/Institution.  
    /// </summary>
    public string? BankName { get; set; }

    /// <summary>
    /// Order in which this setting will appear in the UI.
    /// </summary>
    public short Order { get; set; }

    /// <summary>
    /// URL of the bank's logo.
    /// </summary>
    public string? Logo { get; set; }

    /// <summary>
    /// Currency supported by the bank.
    /// </summary>
    [EnumDataType(typeof(CurrencyTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Name of the bank payment processor.
    /// </summary>
    public string? Processor { get; set; }

    /// <summary>
    /// ID that the processor uses to identify the bank.
    /// </summary>
    public string? ProviderID { get; set; }

    /// <summary>
    /// Whether the bank supports Personal or Business type accounts.
    /// </summary>
    [EnumDataType(typeof(BankAccountTypeEnum))]
    [JsonConverter(typeof(StringEnumConverter))]
    public BankAccountTypeEnum AccountType { get; set; }
}