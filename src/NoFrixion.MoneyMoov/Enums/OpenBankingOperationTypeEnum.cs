// -----------------------------------------------------------------------------
//  Filename: OpenBankingOperationTypeEnum.cs
// 
//  Description: List of open banking operations supported by participating banks.
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  26 Jan 2024     Arif Matin  Created, Harcourt Street, Dublin, Ireland.
// 
//  License: MIT
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

[Flags]
public enum OpenBankingOperationTypeEnum
{
    /// <summary>
    /// No open banking operations supported.
    /// </summary>
    None = 0,

    /// <summary>
    /// Payment initiation services.
    /// </summary>
    PIS = 1,

    /// <summary>
    /// Account information services.
    /// </summary>
    AIS = 2,
}

