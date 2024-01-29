//-----------------------------------------------------------------------------
// Filename: PayrunInvoice.cs
// 
// Description: Represents an invoice that forms part of a Payrun.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 04 Dec 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using NoFrixion.MoneyMoov.Models.Invoices;

namespace NoFrixion.MoneyMoov.Models;

public class PayrunInvoice
{
    public Guid ID { get; set; }
    
    public Guid PayRunID { get; set; }
    
    public string? Name { get; set; }
    
    [Required]
    public required string InvoiceNumber { get; set; }

    public string? PaymentTerms { get; set; }
    
    public DateTimeOffset Date { get; set; }
    
    public DateTimeOffset DueDate { get; set; }
   
    public string Contact { get; set; } = null!;

    public string? DestinationAccountName { get; set; }
    
    public string? DestinationIban { get; set; }
    
    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public CurrencyTypeEnum Currency { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? Taxes { get; set; }
    
    public decimal TotalAmount { get; set; }

    public decimal OutstandingAmount { get; set; }
    
    public decimal? SubTotal { get; set; }
    
    public string? Status { get; set; }

    public string? Reference { get; set; }

    [EmailAddress]
    public required string RemittanceEmail { get; set; }
    
    public IEnumerable<InvoicePayment>? InvoicePayments { get; set; }
}

