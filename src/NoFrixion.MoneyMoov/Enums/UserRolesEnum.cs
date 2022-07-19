//-----------------------------------------------------------------------------
// Filename: UserRolesEnum.cs
//
// Description: An enumeration of the user role types.
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 19 Oct 2021  Donal O'Connor  Created, Carmichael House, Dublin, Ireland.
// 01 Nov 2021  Aaron Clauson   Converted from string constants to enum.
// 19 July 2022 Saurav Maiti    Moved to MoneyMoov SDK

// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums
{
    public enum UserRolesEnum : int
    {
        /// <summary>
        /// The User does not have a roles assigned.
        /// </summary>
        None = 0,

        /// <summary>
        /// User accounts with this role have access to all actions on an account or
        /// merchant that don't require strong customer authentication.
        /// </summary>
        User = 1,

        /// <summary>
        /// User accounts with this role have the same access as the "User" role plus
        /// the additional actions that require strong customer authentication.
        /// </summary>
        Approver = 2
    }
}
