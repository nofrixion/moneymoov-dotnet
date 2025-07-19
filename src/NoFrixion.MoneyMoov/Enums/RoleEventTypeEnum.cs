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
    
    Authorised = 2,
    
    Rejected = 3,
    
    Edited = 4,
    
    Archived = 6,
    
    Unarchived = 7,
    
    AssignedToUser = 8,
    
    RemovedFromUser = 9,
}