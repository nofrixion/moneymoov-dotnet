//-----------------------------------------------------------------------------
// Filename: MerchantUpdate.cs
// 
// Description: Represents an update to a merchant's details.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 26 Jun 2025     Created, Hamilton gardens, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class MerchantUpdate
{
    public string? ShortName { get; set; }
    
    public int? PaymentAccountLimit { get; set; }
    
    public string? LogoUrlPng { get; set; }
    
    public string? LogoUrlSvg { get; set; }

    [MaxLength(2000, ErrorMessage = "Merchant notes must be 2000 characters or less.")]
    public string? Notes { get; set; }
}