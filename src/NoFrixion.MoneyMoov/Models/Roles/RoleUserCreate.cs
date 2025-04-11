// -----------------------------------------------------------------------------
//  Filename: RoleUserCreate.cs
// 
//  Description: Model for creating a new role user:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  19 08 2024  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Roles;

public class RoleUserCreate
{
    public Guid UserID { get; set; }

    public Guid RoleID { get; set; }

    public List<Guid>? Accounts { get; set; }
}