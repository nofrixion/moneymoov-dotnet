// -----------------------------------------------------------------------------
//   Filename: PayrunSourceAccount.cs
// 
//   Description: Payrun source account model:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   7 2 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunSourceAccount
{
    public Guid ID { get; set; }
 
    public CurrencyTypeEnum Currency { get; set; }
    
    public string AccountName { get; set; } = null!;
    
    public decimal AvailableBalance { get; set; }
}