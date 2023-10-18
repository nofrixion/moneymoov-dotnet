// -----------------------------------------------------------------------------
//  Filename: AccountTransactionMetrics.cs
// 
//  Description: Model that represents an account with it's transaction metrics
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  13 10 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

#nullable disable
namespace NoFrixion.MoneyMoov.Models;

public class AccountTransactionMetrics
{
    public Guid AccountID { get; set; }
    
    public string AccountName { get; set; }
    
    public CurrencyTypeEnum Currency { get; set; }
    
    public decimal Balance { get; set; }
    
    public decimal AvailableBalance { get; set; }
    
    public decimal TotalIncomingAmount { get; set; }
    
    public decimal TotalOutgoingAmount { get; set; }
    
    public int NumberOfTransactions { get; set; }
    
    public int NumberOfIncomingTransactions { get; set; }
    
    public int NumberOfOutgoingTransactions { get; set; }
}