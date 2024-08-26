//  -----------------------------------------------------------------------------
//   Filename: PayrunStatus.cs
// 
//   Description: Payrun status enum:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   04 12 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum PayrunStatus
{
    None = 0,
    
    /// <summary>
    /// Indicates that the pay run is in draft mode.
    /// </summary>
    Draft = 1,
    
    /// <summary>
    /// Indicates that all the payouts in the pay run
    /// have been submitted for processing.
    /// </summary>
    Submitted = 2,
    
    /// <summary>
    /// Indicates that the pay run has been completed.
    /// </summary>
    Completed = 3, 
    
    /// <summary>
    /// Indicates that the pay run was rejected by the approver.
    /// </summary>
    Rejected = 4,
    
    /// <summary>
    /// Indicates that approval was requested for the pay run
    /// and is waiting for approval.
    /// </summary>
    AuthorisationPending = 5,
    
    /// <summary>
    /// Indicates that the pay run was approved and payouts were created.
    /// </summary>
    PayoutsCreated = 6,
    
    /// <summary>
    /// Indicates that the pay run was edited.
    /// </summary>
    Edited = 7,
    
    /// <summary>
    /// Indicates that the pay run has been authorised and is queued for processing.
    /// </summary>
    Queued = 8,
}