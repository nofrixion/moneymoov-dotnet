//  -----------------------------------------------------------------------------
//   Filename: ConfigKeys.cs
// 
//   Description: TODO:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   30 08 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public class IdentityConfigKeys
{
    /// <summary>
    /// NoFrixion Identity domain
    /// </summary>
    public const string NOFRIXION_IDENTITY_DOMAIN = "NoFrixionIdentity:Domain";

    /// <summary>
    /// The NoFrixion Identity Client ID.
    /// </summary>
    public const string NOFRIXION_IDENTITY_CLIENTID = "NoFrixionIdentity:ClientId";

    /// <summary>
    /// The NoFrixion Identity Client Secret.
    /// </summary>
    public const string NOFRIXION_IDENTITY_CLIENTSECRET = "NoFrixionIdentity:ClientSecret";

    /// <summary>
    /// The IdentityServer Audience, which identifies the NoFrixion API that users will be calling,
    /// to grant authorised user access to.
    /// </summary>
    public const string IDENTITY_SERVER_AUDIENCE = "NoFrixionIdentity:Audience";
    
    /// <summary>
    /// The IdentityServer Audience for calls to APIs that require Strong Customer Authentication (SCA).
    /// </summary>
    public const string IDENTITY_SERVER_AUDIENCE_STRONG = "NoFrixionIdentity:AudienceStrong";
}