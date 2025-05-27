//-----------------------------------------------------------------------------
// Filename: User.cs
//
// Description: The business layer representation of a user entity record.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 Jan 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Models.Roles;

namespace NoFrixion.MoneyMoov.Models;

public class User
{ 
    public readonly static User Empty = new User();

    public Guid ID { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    public bool TwoFactorEnabled { get; set; }
    
    public bool PasskeyAdded { get; set; }

    public UserPermissions? Permissions { get; set; }
    
    public List<UserRoleWithScope>? RolesWithScope { get; set; }
    
    /// <summary>
    /// The number of seconds a session for this user should last before expiring.
    /// This is based on the user's role on the merchant.
    /// This is used to set the session timeout in the client. If not set the client's default
    /// session timeout will be used. 
    /// </summary>
    public List<ClientSessionTimeout>? ClientSessionTimeouts { get; set; }

    public bool IsEmpty() => ID == Guid.Empty && EmailAddress == string.Empty;
}