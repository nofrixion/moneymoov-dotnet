//  -----------------------------------------------------------------------------
//   Filename: Invoice.cs
// 
//   Description: Invoice model
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

public class Invoice
{
    public Guid ID { get; set; }

    public Guid PayRunID { get; set; }

    public string? Name { get; set; }

    public CurrencyTypeEnum Currency { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal OutstandingAmount { get; set; }

    public decimal? Taxes { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? SubTotal { get; set; }

    public string? DestinationIban { get; set; }

    public string? DestinationAccountName { get; set; }

    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public string Contact { get; set; } = null!;

    public string? PaymentTerms { get; set; }

    public string? Status { get; set; }

    public string? Reference { get; set; }

    public string? RemittanceEmail { get; set; }

    public DateTimeOffset DueDate { get; set; }

    public DateTimeOffset Date { get; set; }

    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
    
    public IEnumerable<InvoicePayment>? Payments { get; set; }
}