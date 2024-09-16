// -----------------------------------------------------------------------------
//  Filename: AccountTypeEnum.cs
// 
//  Description: Provides an enumeration of account types. Mainly for fee calculations.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  16 07 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Enumeration of all possible account types.
/// </summary>
public enum AccountTypeEnum
{
    /// <summary>
    /// Standard accounts. No fees applied.
    /// </summary>
    Standard,
    
    /// <summary>
    /// Standard fee accounts. Fees are applied.
    /// </summary>
    StandardFee,
    
    /// <summary>
    /// Account associated with Tribe.
    /// </summary>
    Tribe,
}