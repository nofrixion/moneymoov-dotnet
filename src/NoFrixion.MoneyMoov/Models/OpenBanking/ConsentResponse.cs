// -----------------------------------------------------------------------------
//  Filename: ConsentResponse.cs
// 
//  Description: The response for an open banking consent request.
//
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  15 Dec 2022  Arif Matin    Created, Harcourt Street, Dublin, Ireland.
//  17 Dec 2022  Aaron Clauson Renamed from AccountAuthorisationResponse to ConsentResponse.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public class ConsentResponse
{
    /// <summary>
    /// The ID of the open banking consent. Once the consent has been authorised this
    /// is the ID that allows it to be utilised via the open banking APIs to list accounts,
    /// transactions etc.
    /// </summary>
    public Guid ConsentID { get; set; }

    /// <summary>
    /// The URL the authorising user needs to be redirected to in order to get the open banking
    /// consent token.
    /// </summary>
    public string AuthorisationUrl { get; set; } = string.Empty;
}
