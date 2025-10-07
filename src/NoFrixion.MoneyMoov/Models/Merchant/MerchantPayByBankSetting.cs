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
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Name of the bank payment processor.
    /// </summary>
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

    /// <summary>
    /// The list of country codes representing the banks the country supports.
    /// </summary>
    public List<string> BankCountryCodes { get; set; } = new List<string>();

    /// <summary>
    /// The heading for a warning message related to the bank institution to be displayed to the user.
    /// </summary>
    public string? WarningHeading { get; set; }

    /// <summary>
    /// The warning message related to the bank institution to be displayed to the user.
    /// </summary>
    public string? WarningMessage { get; set; }
}