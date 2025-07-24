// -----------------------------------------------------------------------------
//  Filename: RoleUserEventTypeEnum.cs
// 
//  Description: List of the types of events that can occur in a role-user assignment:
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  22 07 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum RoleUserEventTypeEnum
{
    None = 0,
    
    AssignedToUser = 1,
    
    EditedAccounts = 2
}