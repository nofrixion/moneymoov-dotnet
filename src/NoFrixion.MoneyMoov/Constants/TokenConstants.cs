// -----------------------------------------------------------------------------
//  Filename: TokenConstants.cs
// 
//  Description: Identity server token constants:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  16 11 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Constants;

public static class TokenConstants
{
    /// <summary>
    /// Standard nofrixion scope for NoFrixion Identity.
    /// </summary>
    public const string NOFRIXION_STANDARD_SCOPE = "nofrixion";

    /// <summary>
    /// Strong nofrixion scope for NoFrixion Identity.
    /// </summary>
    public const string NOFRIXION_STRONG_SCOPE = "nofrixion.strong";

    /// <summary>
    /// Scopes to request when creating a new personal access token for use with
    /// a browser session.
    /// </summary>
    public const string CREATE_STANDARD_TOKEN_SCOPES = "email openid mfa";

    /// <summary>
    /// Scopes to request when creating a new strong access token to approve an
    /// operation requiring strong customer authentication.
    /// </summary>
    public const string CREATE_STRONG_TOKEN_SCOPES = "openid mfa";

    /// <summary>
    /// Scopes to request when creating a new API token (for machine-to-machine applications).
    /// </summary>
    public const string CREATE_MACHINE_TOKEN_SCOPES = "openid offline_access";

    /// <summary>
    /// The name of the query parameter that Identity server returns on a successful login
    /// and that can be used to request a new token.
    /// </summary>
    public const string CODE_KEY_NAME = "code";

    /// <summary>
    /// The name of the query parameter that Identity server uses to return the pseudo-random
    /// state value set when generating a login URL for a new token.
    /// </summary>
    public const string STATE_KEY_NAME = "state";
}