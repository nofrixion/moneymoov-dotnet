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
// 19 July 2022 Saurav Maiti    Moved to Nofrixion.Common

// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum UserRolesEnum : int
{
    /// <summary>
    /// User accounts with this role have access to all actions on an account or
    /// merchant that don't require strong customer authentication.
    /// </summary>
    User = 1,

    /// <summary>
    /// User accounts with this role have the same access as the "User" role plus
    /// the additional actions that require strong customer authentication.
    /// </summary>
    Approver = 2,

    /// <summary>
    /// User accounts with this role have the same access as the "Approver" role plus
    /// the additional admin actions such as assigning or modifying user roles.
    /// </summary>
    AdminApprover = 3,

    /// <summary>
    /// User accounts in this role only have permission to user the payment requests 
    /// to facilitate the receiving of payments. This role has broadly the same capabilities 
    /// granted to a merchant (non-User) token.
    /// </summary>
    PaymentRequestor = 4,

    /// <summary>
    /// For new users that were invited to a merchant and accepted. It's then up to the 
    /// inviter to set the required role they wish them to have. This role has no
    /// permissions.
    /// </summary>
    NewlyRegistered = 5
}
