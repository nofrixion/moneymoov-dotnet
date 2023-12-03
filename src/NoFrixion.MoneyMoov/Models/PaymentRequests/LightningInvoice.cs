//-----------------------------------------------------------------------------
// Filename: LightningInvoice.cs
//
// Description: Represents a Bitcoin \Llighting invoice.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 01 May 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class LightningInvoice
{
    public string Description { get; set; } = string.Empty;

    public string PaymentRequest { get; set; } = string.Empty;

    public string RHash { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }
}
