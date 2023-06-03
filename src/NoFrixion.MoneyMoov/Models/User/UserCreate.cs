//-----------------------------------------------------------------------------
// Filename: UserCreate.cs
//
// Description: Model used to create a new MoneyMov user.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserCreate
{ 
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// The IdentityUserID from the NoFrixion Identity server.
    /// </summary>
    [Required]
    public string IdentityUserID { get; set; } = string.Empty;

    /// <summary>
    /// Optional ID of the invite that was originally sent to the registering
    /// user.
    /// </summary>
    public Guid UserInviteID { get; set; }

    public string? Profile { get; set; }

    /// <summary>
    /// Places all the user create's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the non-collection properties represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(FirstName), FirstName },
            { nameof(LastName), LastName },
            { nameof(EmailAddress), EmailAddress },
            { nameof(IdentityUserID), IdentityUserID },
            { nameof(UserInviteID), UserInviteID.ToString() },
            {nameof(Profile), Profile ?? ""}
        };
    }
}