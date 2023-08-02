//-----------------------------------------------------------------------------
// Filename: PaymentInitiationRequest.cs
// 
// Description: Represents the fields that can be used to initiate a payment
// initiation attempt.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 10 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class PaymentInitiationRequest
{
    /// <summary>
    /// This is the ID of the institution (bank) that the payer ha chosen.
    /// </summary>
    [Required]
    public string ProviderID { get; set; } 

    public decimal PartialAmount { get; set; }

    public string RedirectToOriginUrl { get; set; }
}