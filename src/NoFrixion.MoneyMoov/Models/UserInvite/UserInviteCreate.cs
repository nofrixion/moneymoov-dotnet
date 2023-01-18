//-----------------------------------------------------------------------------
// Filename: UserInviteCreate.cs
// 
// Description: Model to create a new user invite.
//
// Author(s):
// Aaron Clauson    (aaron@nofrixion.com)
// 
// History:
// 29 Dec 2022  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class UserInviteCreate
{
    /// <summary>
    /// ID of the merchant the user is being invited to. Can be empty if
    /// provided by the URL.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Email address of the user being invited.
    /// </summary>
    [Required]
    [EmailAddress]
    public string InviteeEmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// First Name of the user being invited.
    /// </summary>
    public string? InviteeFirstName { get; set; } = string.Empty;

    /// <summary>
    /// Last Name of the user being invited.
    /// </summary>
    public string? InviteeLastName { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL to provide to the invited user to inform them where to
    /// visit to accept the invite.
    /// </summary>
    public string? RegistrationUrl { get; set; } = string.Empty;

    /// <summary>
    /// If set to true an email will be sent to the invitee with instructions on
    /// how to accept the invite.
    /// </summary>
    public bool SendInviteEmail { get; set; }

    /// <summary>
    /// Places all the user invite create's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the non-collection properties represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(MerchantID), MerchantID.ToString() },
            { nameof(InviteeEmailAddress), InviteeEmailAddress },
            { nameof(RegistrationUrl), RegistrationUrl ?? string.Empty },
            { nameof(SendInviteEmail), SendInviteEmail.ToString() },
            {nameof(InviteeFirstName), InviteeFirstName ?? string.Empty},
            {nameof(InviteeLastName), InviteeLastName ?? string.Empty}
        };
    }
}
