// -----------------------------------------------------------------------------
//  Filename: TokenTypesEnum.cs
// 
//  Description: This enum holds the types of access tokens that are used by NoFrixion:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  28 07 2022  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.Biz.TokenAcquirers;

/// <summary>
/// This enum holds the types of access tokens that are used by NoFrixion. The
/// tokens are all access tokens but use a combination of scopes and claims to
/// control how they can be used by NoFrixion users.
/// </summary>
public enum TokenTypesEnum
{
    /// <summary>
    /// A standard access token. Typical use is with a web or single page
    /// application that the user logs into directly with the OAuth Authorisation
    /// Code Flow, see https://auth0.com/docs/authorization/flows/authorization-code-flow.
    /// </summary>
    Standard = 0,

    /// <summary>
    /// An access token used for Strong Customer authentication. These tokens are configured
    /// to be issued with a number of restrictions to make them suitable for ensuring a human
    /// operator is present at the time they are issued:
    /// 
    ///  - Very short expiry (ideally 3 minutes or less),
    ///  - No accompanying refresh token,
    ///  - Include a custom claim so that they can only be used to authorise a single specific
    ///    API action.
    /// </summary>
    Strong = 1,

    /// <summary>
    /// An access token that is intended for use in unattended 3rd party applications. These tokens
    /// are limited to non-critical applications and can be issued with longer expiries.
    /// </summary>
    Machine = 2,

    /// <summary>
    /// A user api token. 
    /// Typical use is with a web or single page application that the user logs into directly with the OAuth Authorisation
    /// Code Flow, see https://auth0.com/docs/authorization/flows/authorization-code-flow.
    /// </summary>
    UserApiToken = 3,
}   