// -----------------------------------------------------------------------------
//  Filename: MerchantPayByBankSettings.cs
// 
//  Description: Model representing a collection of merchant bank payment settings:
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
/// Represents a collection of merchant bank payment settings.
/// </summary>
public class MerchantPayByBankSettings
{
    /// <summary>
    /// Merchant to which the settings will be configured.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Collection of bank payment settings.
    /// </summary>
    public IEnumerable<MerchantPayByBankSetting> PayByBankSettings { get; set; } = new List<MerchantPayByBankSetting>();
}