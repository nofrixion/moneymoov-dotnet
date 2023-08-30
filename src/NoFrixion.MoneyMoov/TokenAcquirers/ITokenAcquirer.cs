//-----------------------------------------------------------------------------
// Filename: ITokenAcquirer.cs
//
// Description: Interface for token acquirers
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 28 Jul 2021  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using NoFrixion.Biz.TokenAcquirers;
using NoFrixion.MoneyMoov.Models.Tokens;

namespace NoFrixion.MoneyMoov.TokenAcquirers;

public interface ITokenAcquirer
{
    string GetLoginRedirectUrl(
        string callbackUrl,
        string state,
        TokenTypesEnum tokenType,
        Dictionary<string, string>? customClaims = null);

    Task<AccessTokenResponse> GetToken(string code, string redirectUrl);

    Task<AccessTokenResponse> RefreshToken(string refreshToken);
}