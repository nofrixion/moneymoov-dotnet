// -----------------------------------------------------------------------------
//  Filename: UserMetrics.cs
// 
//  Description: Represents user metrics for a merchant. Includes user invites as well.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  02 10 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class UserMetrics
{
    /// <summary>
    /// Total user roles and invites count.
    /// </summary>
    public int All { get; set; }

    /// <summary>
    /// Total user invites count.
    /// </summary>
    public int Invited { get; set; }

    /// <summary>
    /// Total user roles with role NewlyRegistered count.
    /// </summary>
    public int RolePending { get; set; }

    /// <summary>
    /// Total user roles with role other than NewlyRegistered count.
    /// </summary>
    public int Active { get; set; }
}