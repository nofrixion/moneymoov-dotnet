//-----------------------------------------------------------------------------
// Filename: UserInvite.cs
// 
// Description: UserInvite model
// Author(s):
// Saurav Maiti      (saurav@nofrixion.com)
// 
// History:
// 19 Aug 2022     Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserInvite
{
    /// <summary>
    /// Number of hours after which a registration invite is considered expired.
    /// The invitee will not be able to register until re-invited.
    /// </summary>
    public const int USER_INVITE_EXPIRATION_HOURS = 72;

    public Guid ID { get; set; }
    public string InviteeEmailAddress { get; set; } = string.Empty;
    public string InviterFirstName { get; set; } = string.Empty;
    public string InviterLastName { get; set; } = string.Empty;
    public string? InviteeFirstName { get; set; } = string.Empty;
    public string? InviteeLastName { get; set; } = string.Empty;
    public string InviterEmailAddress { get; set; } = string.Empty;
    public Guid MerchantID { get; set; }
    public string RegistrationUrl { get; set; } = string.Empty;
    public DateTimeOffset LastInvited { get; set; }
    public string MerchantName { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// If true, indicates the invitee's email address corresponds to an existing MoneyMoov user.
    /// </summary>
    public bool IsInviteeRegistered { get; set; }

    public UserInviteStatusEnum Status
    {
        get
        {
            if ((DateTimeOffset.Now - LastInvited) > new TimeSpan(USER_INVITE_EXPIRATION_HOURS, 0, 0))
            {
                return UserInviteStatusEnum.Expired;
            }
            else
            {
                return UserInviteStatusEnum.Active;
            }
        }
    }
}
