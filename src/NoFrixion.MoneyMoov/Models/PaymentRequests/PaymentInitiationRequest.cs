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

    /// <summary>
    /// Optional. If 0 the full amount is assumed.
    /// </summary>
    public decimal PartialAmount { get; set; }

    [Obsolete("Please use OriginUrl instead.")]
    public string RedirectToOriginUrl 
    {   
        get => OriginUrl; 
        set => OriginUrl = value; 
    }

    /// <summary>
    /// Optional. If set should indicate the origin URL the payer is making the 
    /// payment from. If a pay by bank attempt fails and the payment request does not
    /// have a FailureCallbackUrl set then the payer will be redirected to this URL.
    /// </summary>
    public string OriginUrl { get; set; }
}