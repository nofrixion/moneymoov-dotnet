// -----------------------------------------------------------------------------
//  Filename: ConsentRequest.cs
// 
//  Description: THe request fields needs when requesting an open banking consent token.
//
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  15 Dec 2022  Arif Matin    Created, Harcourt Street, Dublin, Ireland.
//  17 Dec 2022  Aaron Clauson Renamed from AccountAuthorisationRequest toConsentRequest.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public class ConsentRequest
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

    /// <summary>
    /// Optional callback URL that the end user performing the open banking authorisation
    /// will be redirected to on completion. Typically this should be a URL that takes the 
    /// user back to the application that they originally started the open banking authorisation
    /// from.
    /// </summary>
    public string CallbackUrl { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL that will be called by the MoneyMoov server upon a successful open banking
    /// authorisation. The webhook URL will pass a single parameter of "id". That ID can
    /// then be used in the MoneyMoov open banking actions to verify the operation. Note
    /// web hooks can easily be spoofed and should NOT be trusted without calling back to the 
    /// MoneyMoov server for verification.
    /// </summary>
    public string SuccessWebHookUrl { get; set; } = string.Empty;
}
