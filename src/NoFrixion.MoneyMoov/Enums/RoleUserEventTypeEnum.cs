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
    /// A user invite was authorised.
    /// </summary>
    Authorise = 1
}