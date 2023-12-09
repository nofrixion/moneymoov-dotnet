// -----------------------------------------------------------------------------
//  Filename: PayoutReject.cs
// 
//  Description: Contains required properties for a payout reject:
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  08 12 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayoutReject
{
    public string Reason { get; set; } = string.Empty;
}