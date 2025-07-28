// -----------------------------------------------------------------------------
//  Filename: RoleEventTypeEnum.cs
// 
//  Description: List of the types of events that can occur in a role:
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  17 07 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Events that can occur in a role.
/// </summary>
public enum RoleEventTypeEnum
{
    None = 0,

    Created = 1,
    
    Edited = 2,
    
    Archived = 3,
    
    Unarchived = 4,
    
    AssignedToUser = 5,
    
    RemovedFromUser = 6,
    
    UpdatedUserAssignment = 7
}