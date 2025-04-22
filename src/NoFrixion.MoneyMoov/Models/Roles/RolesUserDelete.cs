// -----------------------------------------------------------------------------
//  Filename: RolesUserDelete.cs
// 
//  Description: Model for removing multiple roles from a user.
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
    public class RolesUserDelete
    {
        public List<Guid>? RolesToRemove { get; set; }
    }
}
