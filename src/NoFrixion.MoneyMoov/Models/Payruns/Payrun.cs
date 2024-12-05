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
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models.Approve;

namespace NoFrixion.MoneyMoov.Models;

public class Payrun
{
    public Guid ID { get; set; }

    public Guid? BatchPayoutID { get; set; }

    public string? Name { get; set; }

    public Guid MerchantID { get; set; }

    public List<PayrunInvoice>? Invoices { get; set; } = null!;

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

    public bool IsArchived { get; set; }
    
    public bool CanEdit => Status.CanEdit();
    
    public bool CanDelete => Status.CanDelete();
    
    /// <summary>
    /// The number of authorisers required for this payrun. Is determined by business settings
    /// on the source account and/or merchant.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the payrun.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }
    
    /// <summary>
    /// True if the payrun can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the payrun was loaded for a user and that user has already authorised the latest version of the payrun.
    /// </summary>
    public bool HasCurrentUserAuthorised {  get; set; }

    /// <summary>
    /// A list of the users who have successfully authorised the latest version of the payrun and when.
    /// </summary>
    public List<Authorisation>? Authorisations { get; set; }
}