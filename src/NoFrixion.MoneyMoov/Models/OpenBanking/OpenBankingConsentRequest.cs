// -----------------------------------------------------------------------------
//  Filename: OpenBankingConsentRequest.cs
// 
//  Description: THe request fields needs when requesting an open banking consent token.
//
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  15 Dec 2022  Arif Matin    Created, Harcourt Street, Dublin, Ireland.
//  17 Dec 2022  Aaron Clauson Renamed from AccountAuthorisationRequest to OpenBankingConsentRequest.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class OpenBankingConsentRequest
{
    /// <summary>
    /// The email address that identifies the end user that will be authorising the 
    /// open banking consent request.
    /// </summary>
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// The institution ID the open banking consent is being requested for.
    /// </summary>
    [Required]
    public string InstitutionID { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the merchant the consent token is being created to be used with.
    /// </summary>
    [Required]
    public Guid MerchantID { get; set; }
}
