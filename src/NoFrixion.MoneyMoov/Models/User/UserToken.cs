// -----------------------------------------------------------------------------
// Filename: UserToken.cs
// 
// Description: Represents a UserToken that controls access to the functions
// of the MoneyMoov API that require user level access.
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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserToken
{
    public Guid ID { get; set; }

    public Guid UserID { get; set; }

    public string? Type { get; set; }

    [Required(ErrorMessage = "Description is required")]
    public string? Description { get; set; }

    public string? AccessTokenHash { get; set; }

    public string? RefreshTokenHash { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// This field is used when returning a new user token record to a client. If set it holds the URL
    /// the user needs to visit in order to complete a strong authentication check in order to approve 
    /// the token.
    /// </summary>
    public string? ApproveTokenUrl { get; set; }
}
