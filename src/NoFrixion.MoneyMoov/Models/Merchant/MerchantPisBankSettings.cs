// -----------------------------------------------------------------------------
//  Filename: MerchantPisBankSettings.cs
// 
//  Description: Model representing a collectino of merchant PIS bank settings:
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  08 03 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Represents a collection of merchant PIS bank settings.
/// </summary>
public class MerchantPisBankSettings
{
    /// <summary>
    /// Merchant to which the settings will be configured.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Collection of PIS bank settings.
    /// </summary>
    public IEnumerable<MerchantPisBankSetting> PisBankSettings { get; set; } = new List<MerchantPisBankSetting>();
}