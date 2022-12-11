//-----------------------------------------------------------------------------
// Filename: MoneyMoovConfigKeys.cs
//
// Description: A set of constants representing the configuration settings
// for using the MoneyMoov service.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Jul 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public static class MoneyMoovConfigKeys
{
    /// <summary>
    /// Configuration setting for the NoFrixion MoneyMoov API base URL This is the URL for everything
    /// preceding the resource. 
    /// </summary>
    /// <example>https://api.nofrixion.com/api/v1</example>
    public const string NOFRIXION_MONEYMOOV_API_BASE_URL = "NoFrixion:MoneyMoovApiBaseUrl";
}
