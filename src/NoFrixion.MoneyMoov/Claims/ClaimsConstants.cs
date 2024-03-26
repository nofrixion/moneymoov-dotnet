//-----------------------------------------------------------------------------
// Filename: ClaimsConstants.cs
//
// Description: Contains constants for User claims
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 26 Nov 2021  Donal O'Connor  Created, Carmichael House, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Claims;

public static class ClaimsConstants
{
    public const string NOFRIXION_CLAIMS_NAMESPACE = "https://moneymoov.nofrixion.com/";

    /// <summary>
    /// The prefix NoFrixion Identity use on the name ID scope in their tokens.
    /// </summary>
    public const string NOFRIXION_NAMEID_PREFIX = "nf|";
    
    public const string TWO_FACTOR_ENABLED = "two_factor_enabled";
    
    public const string PASSKEY_ADDED = "passkey_added";
}