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

namespace NoFrixion.MoneyMoov.Models;

public class PayrunInvoice
{
   public string? InvoiceNumber { get; set; }

    public string? PaymentTerms { get; set; }
    
    public DateTimeOffset InvoiceDate { get; set; }
    
    public DateTimeOffset DueDate { get; set; }
   
    public string? Contact { get; set; }
    
    public string? DestinationIban { get; set; }
    
    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public CurrencyTypeEnum Currency { get; set; }
    
    public decimal? Subtotal { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? Taxes { get; set; }
    
    public decimal? TotalAmount { get; set; }

    public decimal? OutstandingAmount { get; set; }
    
    public string? InvoiceStatus { get; set; }

    public string? Reference { get; set; }

    public string? RemittanceEmail { get; set; }
}

