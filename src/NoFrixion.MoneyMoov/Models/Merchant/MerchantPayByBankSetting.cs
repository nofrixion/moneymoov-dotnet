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
    [EnumDataType(typeof(PaymentProcessorsEnum))]
    [JsonConverter(typeof(PaymentProcessorsEnum))]
    public PaymentProcessorsEnum Processor { get; set; }

    /// <summary>
    /// ID that the processor uses to identify the bank (personal accounts).
    /// </summary>
    public string? PersonalInstitutionID { get; set; }

    /// <summary>
    /// ID that the processor uses to identify the bank (business accounts).
    /// </summary>
    public string? BusinessInstitutionID { get; set; }

    /// <summary>
    /// Message relating to specific bank.  
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Optional image URL to be displayed with the message.
    /// </summary>
    public string? MessageImageUrl { get; set; }
}