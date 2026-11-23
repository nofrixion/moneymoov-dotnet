//-----------------------------------------------------------------------------
// Filename: SkippedInvoice.cs
//
// Description: Represents an invoice that was excluded from a payrun because
// it was missing required address fields.
//
// License: MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class SkippedInvoice
{
    /// <summary>
    /// The invoice reference from the source system (e.g. Xero invoice number).
    /// </summary>
    public string? InvoiceReference { get; set; }

    /// <summary>
    /// The payee account name on the invoice.
    /// </summary>
    public string? DestinationAccountName { get; set; }

    /// <summary>
    /// Human-readable reason why this invoice was excluded from the payrun.
    /// Intended to be shown directly to the user.
    /// </summary>
    public string? SkipReason { get; set; }
}
