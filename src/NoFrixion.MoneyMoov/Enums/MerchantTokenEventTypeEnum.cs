// -----------------------------------------------------------------------------
//  Filename: MerchantTokenEventTypeEnum.cs
// 
//  Description: Enum for the different types of merchant token events.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  23 01 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum MerchantTokenEventTypeEnum
{
    /// <summary>
    /// Something went wrong and the event type is unknown.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// A merchant token was created.
    /// </summary>
    Create = 1,
    
    /// <summary>
    /// A merchant token was authorised by an approver.
    /// </summary>
    Authorise = 2,
    
    /// <summary>
    /// A merchant token was automatically enabled after completion of the authorisation process.
    /// </summary>
    Enable = 3,
    
    /// <summary>
    /// A merchant token was archived.
    /// </summary>
    Archive = 4,
    
    /// <summary>
    /// A merchant token was edited.
    /// </summary>
    Edited = 5
}