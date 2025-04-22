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
    /// Standard fee accounts. Fees are collected from all accounts and
    /// sent to a specific IBAN/SCAN.
    /// </summary>
    StandardFee,
    
    /// <summary>
    /// For liquidator customers who have a fee collection account set up.
    /// Fees are collected from all other accounts and sent to the collection account. 
    /// </summary>
    LiquidatorFee,
}