// -----------------------------------------------------------------------------
//  Filename: AccessTokenResponse.cs
// 
//  Description: Represents an access token response:
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

namespace NoFrixion.MoneyMoov.Models.Tokens;

public class AccessTokenResponse
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public int? ExpiresIn { get; set; }
}