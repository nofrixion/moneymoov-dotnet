// -----------------------------------------------------------------------------
//  Filename: AccountSepaInstantStatusEnum.cs
// 
//  Description: Enum for the status of SEPA Instant availability for an account.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  08 01 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Enum for the status of SEPA Instant availability for an account.
/// </summary>
public enum AccountSepaInstantStatusEnum
{
    /// <summary>
    /// Default state.
    /// </summary>
    None,
    
    /// <summary>
    /// Account hasn't been sent to supplier to enable SEPA Instant yet.
    /// </summary>
    Pending,
    
    /// <summary>
    /// Supplier is in the process of enabling SEPA Instant.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Account is enabled for SEPA Instant.
    /// </summary>
    Enabled,
    
    /// <summary>
    /// For some reason SEPA Instant is not enabled for this account.
    /// </summary>
    Disabled,
    
    /// <summary>
    /// Supplier failed to enable SEPA Instant for this account.
    /// </summary>
    Failed
}