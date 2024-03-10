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

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class Payrun
{
    public Guid ID { get; set; }

    public Guid? BatchPayoutID { get; set; }

    public string? Name { get; set; }

    public Guid MerchantID { get; set; }

    public List<PayrunInvoice> Invoices { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }

    public DateTimeOffset? ScheduleDate { get; set; }

    public PayrunStatus Status { get; set; }

    public List<PaymentAccount> SourceAccounts { get; set; } = new();
    
    public User? LastUpdatedBy { get; set; } = null!;
    
    public List<PayrunEvent> Events { get; set; } = new();
    
    public List<Payout>? Payouts { get; set; }
    
    public List<PayrunPayment>? Payments { get; set; }
}