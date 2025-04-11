// -----------------------------------------------------------------------------
//  Filename: RolesUserUpdate.cs
// 
//  Description: Model to update multiple roles for a user.
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  11 Apr 2025  Arif Matin   Created, Belvedere road, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles
{
    public class RolesUserUpdate
    {
        public List<RoleUserUpdate>? RolesToUpdate { get; set; }
    }
}
