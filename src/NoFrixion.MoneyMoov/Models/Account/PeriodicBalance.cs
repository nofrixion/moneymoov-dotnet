// -----------------------------------------------------------------------------
//  Filename: PeriodicBalance.cs
// 
//  Description: Model representing balance at a particular period of time.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  24 10 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PeriodicBalance
{
    /// <summary>
    /// The date and time of the balance. For daily balances this will be the end of the day.
    /// </summary>
    public DateTimeOffset BalanceAt { get; set; }
    
    /// <summary>
    /// The balance amount.
    /// </summary>
    public decimal Balance { get; set; }
}