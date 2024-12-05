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

    public List<UserRole> Roles { get; set; } = new List<UserRole>();
    
    public bool TwoFactorEnabled { get; set; }
    
    public bool PasskeyAdded { get; set; }

    public UserRolePermissions? Permissions { get; set; }
    
    public List<UserRoleMinimal>? UserRoles { get; set; }
    
    public bool IsEmpty() => ID == Guid.Empty && EmailAddress == string.Empty;
}

public class UserRoleMinimal
{
    public Guid RoleID { get; set; }
    
    public string RoleName { get; set; } = string.Empty;
    
    public List<Guid>? AccountIDs { get; set; }
}