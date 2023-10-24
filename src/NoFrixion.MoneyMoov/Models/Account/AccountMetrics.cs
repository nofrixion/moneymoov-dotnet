// -----------------------------------------------------------------------------
//  Filename: AccountMetrics.cs
// 
//  Description: Entity containing account metrics of a merchant..
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  18 10 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class AccountMetrics
{
    public Guid MerchantID { get; set; }

    public CurrencyTypeEnum Currency { get; set; }

    public int NumberOfAccounts { get; set; }

    public decimal TotalBalance { get; set; }

    public decimal TotalAvailableBalance { get; set; }
    
    public List<PeriodicBalance>? PeriodicBalances { get; set; }
    
    public DateTimeOffset? PeriodicBalancesFromDate { get; set; }
    
    public DateTimeOffset? PeriodicBalancesToDate { get; set; }
    
    public TimeFrequencyEnum PeriodicBalancesFrequency { get; set; }
}