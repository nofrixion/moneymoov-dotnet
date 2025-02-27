//-----------------------------------------------------------------------------
// Filename: UserInviteStatusEnum.cs
// 
// Description: Enum for statuses of user invites.
// Author(s):
// Saurav Maiti      (saurav@nofrixion.com)
// 
// History:
// 22 Aug 2022     Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum UserInviteStatusEnum
{
    /// <summary>
    /// Registration invite is active i.e 
    /// 48 hours have not passed since the invite was sent.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Registration invite has expired i.e 
    /// 48 hours have passed since the invite was sent.
    /// </summary>
    Expired = 1,
    
    /// <summary>
    /// User has accepted the invite but the role is pending.
    /// </summary>
    Accepted = 2
}
