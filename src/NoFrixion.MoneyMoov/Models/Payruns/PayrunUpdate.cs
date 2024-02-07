// -----------------------------------------------------------------------------
//   Filename: PayrunUpdate.cs
// 
//   Description: Represent the model to update a Payrun:
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

public class PayrunUpdate
{
    public Guid ID { get; set; }
    
    public string? Name { get; set; }
    
    public Dictionary<CurrencyTypeEnum, Guid>? SourceAccounts { get; set; }
    
    public Dictionary<Guid, bool>? Invoices { get; set; }
    
    public DateTimeOffset? ScheduledDate { get; set; }
}