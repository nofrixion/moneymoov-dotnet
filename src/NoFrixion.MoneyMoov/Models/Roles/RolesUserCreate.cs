// -----------------------------------------------------------------------------
//  Filename: RolesUserCreate.cs
// 
//  Description: Model for assigning multiple roles to a user.
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  09 Apr 2025  Arif Matin   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles
{
    public class RolesUserCreate
    {
        public  List<RoleUserCreate>? RolesUser { get; set; }
    }
}