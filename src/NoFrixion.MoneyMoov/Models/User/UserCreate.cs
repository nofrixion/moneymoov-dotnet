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
using System.Text.Json.Serialization;

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

    /// <summary>
    /// The IdentityUserID for a newly created user. This field is only required
    /// when a new user is being created. It doesn't need to be sent.
    /// </summary>
    [JsonIgnore]
    public string IdentityUserID { get; set; } = string.Empty;

    /// <summary>
    /// The company name specified by a newly registered sandbox user. This field
    /// is only required when a new user is being created. It doesn't need to be sent. 
    /// </summary>
    [JsonIgnore]
    public string CompanyName { get; set; } = string.Empty;

    public bool IsEmpty() => ID == Guid.Empty && EmailAddress == string.Empty;

    /// <summary>
    /// The merchant Id to add user role for a newly created user. This field
    /// is only required when a new user is being created. It doesn't need to be sent. 
    /// </summary>
    [JsonIgnore]
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Flag indicating if a user has a pending invitation.
    /// </summary>
    public bool IsInvitee { get; set; }
}