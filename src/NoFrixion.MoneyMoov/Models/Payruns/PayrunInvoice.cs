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
    
    [Required]
    public required string InvoiceNumber { get; set; }

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

    public string? Reference { get; set; }

    [EmailAddress]
    public string? RemittanceEmail { get; set; }
    
    public IEnumerable<InvoicePayment>? InvoicePayments { get; set; }
    
    public bool IsEnabled { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (TotalAmount <= decimal.Zero)
        {
            yield return new ValidationResult("The TotalAmount must be more than 0.", new string[] { nameof(TotalAmount) });
        }

        if (string.IsNullOrEmpty(DestinationIban) && string.IsNullOrEmpty(DestinationAccountNumber))
        {
            yield return new ValidationResult("The DestinationIban or DestinationAccountNumber must be provided.", new string[] { nameof(DestinationIban), nameof(DestinationAccountNumber) });
        }
        
        if (!string.IsNullOrEmpty(DestinationIban) && !string.IsNullOrEmpty(DestinationAccountNumber))
        {
            yield return new ValidationResult("The DestinationIban and DestinationAccountNumber cannot both be provided.", new string[] { nameof(DestinationIban), nameof(DestinationAccountNumber) });
        }
        
        if (!string.IsNullOrEmpty(DestinationIban) && !PayoutsValidator.ValidateIBAN(DestinationIban))
        {
            yield return new ValidationResult("The DestinationIban must be a valid IBAN.", new string[] { nameof(DestinationIban) });
        }
        
        if (!string.IsNullOrEmpty(DestinationAccountNumber) && string.IsNullOrEmpty(DestinationSortCode))
        {
            yield return new ValidationResult("DestinationSortCode must be provided if DestinationAccountNumber is set.", new string[] { nameof(DestinationSortCode) });
        }
        
        if (!string.IsNullOrEmpty(DestinationSortCode) && string.IsNullOrEmpty(DestinationAccountNumber))
        {
            yield return new ValidationResult("DestinationAccountNumber must be provided if DestinationSortCode is set.", new string[] { nameof(DestinationSortCode) });
        }
        
        if (!string.IsNullOrEmpty(DestinationIban) && Currency != CurrencyTypeEnum.EUR)
        {
            yield return new ValidationResult("DestinationIban can only be set for EUR currency.", new string[] { nameof(DestinationIban) });
        }
        
        if (!string.IsNullOrEmpty(DestinationSortCode) && !string.IsNullOrEmpty(DestinationAccountNumber) && Currency != CurrencyTypeEnum.GBP)
        {
            yield return new ValidationResult("DestinationSortCode and DestinationAccountNumber can only be set for GBP currency.", new string[] { nameof(DestinationSortCode), nameof(DestinationAccountNumber) });
        }

        if (!PayoutsValidator.IsValidAccountName(DestinationAccountName ?? string.Empty))
        {
            yield return new ValidationResult("DestinationAccountName must be a valid account name.", new string[] { nameof(DestinationAccountName) });
        }
    }
}