// -----------------------------------------------------------------------------
// Filename: UserTokenUpdate.cs
// 
// Description: Respresents the fields of a UserToken that can be updated.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 18 Nov 2021 Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserTokenUpdate
{
    public string? Description { get; set; }

    public string? AccessTokenHash { get; set; }

    public string? RefreshTokenHash { get; set; }

    /// <summary>
    /// Places all the user token update's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the user token update's properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (Description != null) dict.Add(nameof(Description), Description);
        if (AccessTokenHash != null) dict.Add(nameof(AccessTokenHash), AccessTokenHash);
        if (RefreshTokenHash != null) dict.Add(nameof(RefreshTokenHash), RefreshTokenHash);

        return dict;
    }
}
