//  -----------------------------------------------------------------------------
//   Filename: Payrun.cs
// 
//   Description: Payrun model
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   04 12 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   MIT.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class Payrun
{
    public Guid ID { get; set; }
    public string? Name { get; set; }
    public Guid MerchantID { get; set; }

    public List<PayrunInvoice> Invoices { get; set; } = null!;

    public decimal TotalAmount { get; set; }
    public DateTimeOffset Inserted { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset? ScheduleDate { get; set; }
}