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
    
    public Guid ID { get; set; }
    
    /// <summary>
    /// The ID of the pay run this invoice belongs to.
    /// </summary>
    public Guid PayRunID { get; set; }
    
    /// <summary>
    /// Optional. The name of the invoice.
    /// </summary>
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

    /// <summary>
    /// Optional additional information about the invoice.
    /// </summary>
    public string? PaymentTerms { get; set; }
    
    /// <summary>
    /// Date the invoice was issued.
    /// </summary>
    public DateTimeOffset Date { get; set; }
    
    /// <summary>
    /// Date by which payment is due.
    /// </summary>
    public DateTimeOffset DueDate { get; set; }
   
    /// <summary>
    /// Supplier's name
    /// </summary>
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

    /// <summary>
    /// The destination details that will receive the payment for this invoice.
    /// </summary>
    public Counterparty? Destination { get; set; }
    
    /// <summary>
    /// Currency used in the invoice.
    /// </summary>
    [Required]
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Optional. Any discounts applied to the invoice.
    /// </summary>
    public decimal? Discounts { get; set; }

    /// <summary>
    /// Optional. The total amount of taxes applied to the invoice.
    /// </summary>
    public decimal? Taxes { get; set; }
    
    /// <summary>
    /// Final total amount of the invoice, including discounts and taxes.
    /// </summary>
    [Required]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// The amount that is still outstanding for this invoice.
    /// </summary>
    public decimal OutstandingAmount { get; set; }
    
    /// <summary>
    /// Optional. The total amount before discounts and taxes.
    /// </summary>
    public decimal? SubTotal { get; set; }
    
    public string? Status { get; set; }
    
    /// <summary>
    /// Email address to which a remittance note will be sent, if specified.
    /// </summary>
    [EmailAddressMultiple(ErrorMessage = PayrunConstants.REMMITANCE_EMAIL_ADDRESSES_ERROR_MESSAGE)]
    public string? RemittanceEmail { get; set; }
    
    public Guid? XeroInvoiceID { get; set; }
    
    public IEnumerable<InvoicePayment>? InvoicePayments { get; set; }
    
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// The reference sent to the recipient's bank. If left empty,
    /// an autogenerated reference will be used instead.
    /// </summary>
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