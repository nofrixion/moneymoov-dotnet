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
    public Guid UserIniviteID { get; set; }
}