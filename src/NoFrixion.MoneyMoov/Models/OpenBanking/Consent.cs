// -----------------------------------------------------------------------------
//  Filename: Consent.cs
// 
//  Description: Represents an open banking consent token that, when authorised,
//  can be used to call the open banking APIs, to retrieve account information,
// list transactions etc.
//
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  15 Dec 2022  Arif Matin     Created, Belvedere Road, Dublin, Ireland.
//  17 Dec 2022  Aaron Clauson  Renamed from OpenBanking to Consent.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.OpenBanking;

public class Consent
{
    public Guid ID { get; set; }
    public string InstitutionID { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public string CallbackUrl { get; set; } = string.Empty;
    public string SuccessWebHookUrl { get; set; } = string.Empty;
    public PaymentProcessorsEnum Provider { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset Inserted { get; set; }

    public Uri GetSuccessWebhookUri()
    {
        if (string.IsNullOrEmpty(SuccessWebHookUrl))
        {
            return new Uri(MoneyMoovConstants.SUCCESS_WEBHOOK_BLACKHOLE_URI);
        }
        else
        {
            var successWebHookUri = new UriBuilder(SuccessWebHookUrl);

            string successParams = $"id={ID}";

            successWebHookUri.Query = string.IsNullOrEmpty(successWebHookUri.Query)
                ? successParams
                : successWebHookUri.Query + "&" + successParams;

            return successWebHookUri.Uri;
        }
    }
}
