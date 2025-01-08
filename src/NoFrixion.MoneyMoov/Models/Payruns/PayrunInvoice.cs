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
using NoFrixion.MoneyMoov.Attributes;
using NoFrixion.MoneyMoov.Constants;
using NoFrixion.MoneyMoov.Models.Invoices;

namespace NoFrixion.MoneyMoov.Models;

public class PayrunInvoice : IValidatableObject
{
    public const int PAYRUN_INVOICE_PAYMENT_REFERENCE_MAX_LENGTH = 18;
    
    public Guid ID { get; set; }
    
    public Guid PayRunID { get; set; }
    
    public string? Name { get; set; }
    
    [Obsolete("Please use InvoiceReference instead.")]
    public string? InvoiceNumber { get; set; }
    
    [Obsolete("Please use InvoiceReference instead.")]
    public string? Reference
    {
        get => InvoiceReference;
        init => InvoiceReference = value ?? string.Empty;
    }

    [Required]
    [MinLength(1, ErrorMessage = "InvoiceReference cannot be empty.")]
    public string InvoiceReference { get; set; } = null!;

    public string? PaymentTerms { get; set; }
    
    public DateTimeOffset Date { get; set; }
    
    public DateTimeOffset DueDate { get; set; }
   
    public string Contact { get; set; } = null!;

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountName
    {
        get => Destination?.Name;
        set
        {
            Destination ??= new Counterparty();
            Destination.Name = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationIban 
    {
        get => Destination?.Identifier?.IBAN;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.EUR };
            Destination.Identifier.IBAN = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationAccountNumber
    {
        get => Destination?.Identifier?.AccountNumber;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.AccountNumber = value;
        }
    }

    [Obsolete("Please use Destination.")]
    public string? DestinationSortCode
    {
        get => Destination?.Identifier?.SortCode;
        set
        {
            Destination ??= new Counterparty();
            Destination.Identifier ??= new AccountIdentifier { Currency = CurrencyTypeEnum.GBP };
            Destination.Identifier.SortCode = value;
        }
    }

    public Counterparty? Destination { get; set; }

    [Required]
    public CurrencyTypeEnum Currency { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? Taxes { get; set; }
    
    [Required]
    public decimal TotalAmount { get; set; }

    public decimal OutstandingAmount { get; set; }
    
    public decimal? SubTotal { get; set; }
    
    public string? Status { get; set; }
    
    [EmailAddressMultiple(ErrorMessage = PayrunConstants.REMMITANCE_EMAIL_ADDRESSES_ERROR_MESSAGE)]
    public string? RemittanceEmail { get; set; }
    
    public Guid? XeroInvoiceID { get; set; }
    
    public IEnumerable<InvoicePayment>? InvoicePayments { get; set; }
    
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// Represents the reference used in the payout created for this invoice.
    /// For a single destination (e.g., multiple invoices with the same IBAN),
    /// the PaymentReference should remain consistent across all invoices.
    /// If the PaymentReference is not set, one will be generated automatically.
    /// </summary>
    [MaxLength(PAYRUN_INVOICE_PAYMENT_REFERENCE_MAX_LENGTH, ErrorMessage = "PaymentReference cannot be longer than 18 characters.")]
    public string? PaymentReference { get; set; }
    
    /// <summary>
    /// If this invoice was created from an external invoice, this will be the ID of the external invoice.
    /// </summary>
    public string? ExternalInvoiceID { get; set; }
    
    /// <summary>
    /// If this invoice was created from an external invoice, this will be the provider of the external invoice.
    /// E.g., "Xero", "QuickBooks", etc.
    /// </summary>
    public string? ExternalInvoiceProvider { get; set; }

    public NoFrixionProblem Validate()
    {
        var results = new List<ValidationResult>();
        var contextInvoice = new ValidationContext(this, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(this, contextInvoice, results, true);
        
        return isValid ? NoFrixionProblem.Empty : new NoFrixionProblem("The PayrunInvoice model has one or more validation errors.", results);
    }

    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        => this.ToPayout().Validate(validationContext);
}