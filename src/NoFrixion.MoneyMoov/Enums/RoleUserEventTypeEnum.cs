//-----------------------------------------------------------------------------
// Filename: RoleUserEventTypeEnum.cs
// 
// Description: Enum for the different types of role user (assigning a user to a role)
// events that can occur.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 22 Aug 2025  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum RoleUserEventTypeEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// A role-user assignment was authorised by an approver.
    /// </summary>
    Authorise = 1,

    /// <summary>
    /// A user was assigned to a role — the RoleUser record was created.
    /// </summary>
    Created = 2,

    /// <summary>
    /// The account-level permissions on this role-user assignment were updated.
    /// </summary>
    Updated = 3,

    /// <summary>
    /// The user was removed from the role — the RoleUser record was deleted.
    /// </summary>
    Deleted = 4
}