// -----------------------------------------------------------------------------
//  Filename: PayrunInvoiceMinimal.cs
// 
//  Description: Represents a minimal Invoice model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  23 05 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Invoices;

public class PayrunInvoiceMinimal
{
    /// <summary>
    /// The invoice id.
    /// </summary>
    public Guid ID { get; set; }
    
    /// <summary>
    /// If this invoice was created from an external invoice, this will be the ID of the external invoice.
    /// </summary>
    public string? ExternalInvoiceID { get; set; }
    
    /// <summary>
    /// If this invoice was created from an external invoice, this will be the provider of the external invoice.
    /// E.g., "Xero", "QuickBooks", etc.
    /// </summary>
    public string? ExternalInvoiceProvider { get; set; }
}