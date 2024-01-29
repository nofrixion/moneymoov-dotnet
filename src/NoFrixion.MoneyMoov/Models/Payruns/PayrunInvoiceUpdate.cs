// -----------------------------------------------------------------------------
// Filename: PayrunInvoiceUpdate.cs
// 
// Description: Contains details to update the invoice's amount to pay request
//
// Author(s):
// Pablo Maldonado (pablo@nofrixion.com)
// 
// History:
//  26 Jan 2024  Pablo Maldonado   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class PayrunInvoiceUpdate
{
    public string? Name { get; set; }

    public string? InvoiceNumber { get; set; }

    public string? PaymentTerms { get; set; }

    public DateTimeOffset? Date { get; set; }

    public DateTimeOffset? DueDate { get; set; }

    public string? Contact { get; set; }

    public string? DestinationAccountName { get; set; }

    public string? DestinationIban { get; set; }

    public string? DestinationAccountNumber { get; set; }

    public string? DestinationSortCode { get; set; }

    public CurrencyTypeEnum? Currency { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? Taxes { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "TotalAmount must be greater than or equal to 0")]
    public decimal? TotalAmount { get; set; }

    public decimal? OutstandingAmount { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "AmountToPay must be greater than or equal to 0")]
    public decimal? AmountToPay { get; set; }

    public decimal? SubTotal { get; set; }

    public string? Reference { get; set; }

    public string? RemittanceEmail { get; set; }
}