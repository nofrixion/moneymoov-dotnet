// -----------------------------------------------------------------------------
//  Filename: MerchantSuspend.cs
// 
//  Description: Class containing information for a merchant to be suspended:
// 
//  Author(s):
//  Pablo Maldonado (pablo@nofrixion.com)
// 
//  History:
//  18 03 2025  Pablo Maldonado   Created, Montevideo, Uruguay.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class MerchantSuspend
{
    /// <summary>
    /// The reason for the suspension.
    /// </summary>
    public string? Reason { get; set; }

}