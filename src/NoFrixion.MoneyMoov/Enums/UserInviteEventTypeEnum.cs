//-----------------------------------------------------------------------------
// Filename: UserInviteEventTypeEnum.cs
// 
// Description: Enum for the different types of user invite events that can
// occur.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 Aug 2025  Aaron Clauson   Created, Carnesore Point, Wexford, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum UserInviteEventTypeEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A user invite was created and stored.
    /// </summary>
    Created = 1,

    /// <summary>
    /// A user invite was authorised by an approver.
    /// </summary>
    Authorise = 2,

    /// <summary>
    /// The invite email was sent or resent to the invitee.
    /// </summary>
    Sent = 3,

    /// <summary>
    /// The invitee accepted the invite and completed registration.
    /// </summary>
    Accepted = 4,

    /// <summary>
    /// The user invite was hard-deleted by an administrator.
    /// </summary>
    Deleted = 5
}