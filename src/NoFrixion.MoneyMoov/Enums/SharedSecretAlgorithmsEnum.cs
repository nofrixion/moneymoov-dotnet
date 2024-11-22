//-----------------------------------------------------------------------------
// Filename: SharedSecretAlgorithmsEnum.cs
// 
// Description: A list of the shared secret algorithms supported. Original use
// case was for adding an HMAC option to the merchant tokens.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// Halloween 2024   Aaron Clauson   Created, Carne, Wexford, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum SharedSecretAlgorithmsEnum
{
    None,
    
    /// <summary>
    /// Only supported for legacy reasons. NOT supported for merchant or api token HMACs
    /// </summary>
    HMAC_SHA1,

    HMAC_SHA256,
    HMAC_SHA384,

    /// <summary>
    /// Recommended.
    /// </summary>
    HMAC_SHA512
}