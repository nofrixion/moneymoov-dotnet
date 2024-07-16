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

public class PayrunInvoice : IValidatableObject
{
    public Guid ID { get; set; }
    
    public Guid PayRunID { get; set; }
    
    public string? Name { get; set; }
    
    [Obsolete("Please use Reference instead.")]
    public string? InvoiceNumber { get; set; }
    
    public required string Reference { get; set; }

    public string? PaymentTerms { get; set; }
    
    public DateTimeOffset Date { get; set; }
    
    public DateTimeOffset DueDate { get; set; }
   
    public string Contact { get; set; } = null!;

    [Required]
    public string? DestinationAccountName { get; set; }
    
    public string? DestinationIban { get; set; }
    
    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    [Required]
    public CurrencyTypeEnum Currency { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? Taxes { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }

    public decimal OutstandingAmount { get; set; }
    
    public decimal? SubTotal { get; set; }
    
    public string? Status { get; set; }

    [EmailAddress]
    public string? RemittanceEmail { get; set; }
    
    public IEnumerable<InvoicePayment>? InvoicePayments { get; set; }
    
    public bool IsEnabled { get; set; }

    public NoFrixionProblem Validate()
        => this.ToPayout().Validate();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => this.ToPayout().Validate(validationContext);
}