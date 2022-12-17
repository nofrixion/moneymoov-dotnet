// -----------------------------------------------------------------------------
//  Filename: OpenBankingConsent.cs
// 
//  Description: Represetns an open banking consent token that, when authorised,
//  can be used to call the open banking APIs, to retrieve account information,
// list transactions etc.
//
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  15 Dec 2022  Arif Matin     Created, Belvedere Road, Dublin, Ireland.
//  17 Dec 2022  Aaron Clauson  Renamed from OpenBanking to OpenBankingConsent.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class OpenBankingConsent
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
}
