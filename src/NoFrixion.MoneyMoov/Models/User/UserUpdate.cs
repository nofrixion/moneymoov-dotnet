//-----------------------------------------------------------------------------
// Filename: UserUpdate.cs
//
// Description: Model used to update MoneyMov user.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 03 Jun 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserUpdate
{ 
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    [EmailAddress]
    public string? EmailAddress { get; set; }

    /// <summary>
    /// Optional ID of the invite that is being accepted so the user can be assigned
    /// a role on a new merchant.
    /// </summary>
    public Guid UserInviteID { get; set; }

    public string? Profile { get; set; }

    /// <summary>
    /// Places all the user token update's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the user token update's properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        if (FirstName != null) dict.Add(nameof(FirstName), FirstName);
        if (LastName != null) dict.Add(nameof(LastName), LastName);
        if (EmailAddress != null) dict.Add(nameof(EmailAddress), EmailAddress);
        if (Profile != null) dict.Add(nameof(Profile), Profile);
        if (UserInviteID != Guid.Empty) dict.Add(nameof(UserInviteID), UserInviteID.ToString());

        return dict;
    }
}