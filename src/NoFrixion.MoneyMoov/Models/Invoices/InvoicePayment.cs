//  -----------------------------------------------------------------------------
//   Filename: InvoicePayment.cs
// 
//   Description: Invoice payment model:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   04 12 2023  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Invoices;

public class InvoicePayment
{
    public Guid ID { get; set; }

    public Guid InvoiceID { get; set; }

    public decimal Amount { get; set; }

    public Guid? PayoutID { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}