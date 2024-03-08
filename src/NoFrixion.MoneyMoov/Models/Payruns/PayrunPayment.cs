// -----------------------------------------------------------------------------
//   Filename: PayrunPayment.cs
// 
//   Description: Payrun payment model:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   8 3 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayrunPayment
{
    public decimal Amount { get; set; }
    
    public CurrencyTypeEnum Currency { get; set; }
    
    public Counterparty? Destination { get; set; }
}