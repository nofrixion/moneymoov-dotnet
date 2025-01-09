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
    /// Unknown state.
    /// </summary>
    Unknown,
    
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
    Disabled
}