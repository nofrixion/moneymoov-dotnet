// -----------------------------------------------------------------------------
//  Filename: XeroBankFeedConnectionStatusEnum.cs
// 
//  Description: Defines all the possible statuses for a Xero bank feed connection.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  09 09 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Defines all the possible statuses for a Xero bank feed connection.
/// </summary>
public enum XeroBankFeedConnectionStatusEnum
{
    /// <summary>
    /// Default status. No status has been set.
    /// </summary>
    None,
    
    /// <summary>
    /// The bank feed connection has been queued for processing.
    /// </summary>
    Pending,
    
    /// <summary>
    /// The bank feed connection is fully operational.
    /// </summary>
    Active,
    
    /// <summary>
    /// This means that the respective account on Xero has been Archived/Deleted.
    /// This status will be used when the Xero bank feed connection ID can no longer be found on Xero.
    /// </summary>
    Inactive,
    
    /// <summary>
    /// The bank feed had a failure during the connection process.
    /// </summary>
    Failed
}