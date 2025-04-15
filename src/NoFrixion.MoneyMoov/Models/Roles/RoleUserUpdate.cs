// -----------------------------------------------------------------------------
//  Filename: RoleUserUpdate.cs
// 
//  Description: Role user update model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  26 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUserUpdate
{
    public Guid RoleID { get; set; }

    /// <summary>
    /// The accounts the user will have permssions for from the associated role
    /// </summary>
    public List<Guid>? RoleAccounts { get; set; }
}