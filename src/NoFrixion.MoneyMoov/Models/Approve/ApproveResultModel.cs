//-----------------------------------------------------------------------------
// Filename: ApproveResultModel.cs
// 
// Description: The properties returned as claims from the identity server for
// an approval operation.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 16 Apr 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Security.Claims;
using NoFrixion.MoneyMoov.Claims;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models.Approve;

public class ApproveResultModel
{
    public Guid ID { get; set; }

    public ApproveTypesEnum ApproveType { get; set; }

    public string ChallengeBase64Url { get; set; } = string.Empty;

    public string Signature { get; set; } = string.Empty;

    public string KeyID { get; set; } = string.Empty;

    public ApproveResultModel()
    { }

    public ApproveResultModel(IEnumerable<Claim> claims)
    {
        if (claims != null)
        {
            var approveIDClaim = claims.FirstOrDefault(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveid)?.Value;
            if (!string.IsNullOrEmpty(approveIDClaim) && Guid.TryParse(approveIDClaim, out var id))
            {
                ID = id;
            }

            var approveTypeClaim = claims.FirstOrDefault(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvetype)?.Value;
            if (!string.IsNullOrEmpty(approveTypeClaim) && Enum.TryParse<ApproveTypesEnum>(approveTypeClaim, out var approveType))
            {
                ApproveType = approveType;
            }

            ChallengeBase64Url = claims.FirstOrDefault(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvehash)?.Value ?? string.Empty;
            Signature = claims.FirstOrDefault(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvesignature)?.Value ?? string.Empty;
            KeyID = claims.FirstOrDefault(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvekeyid)?.Value ?? string.Empty;
        }
    }

    public ClaimsIdentity UpdateIdentity(ClaimsIdentity claimsIdentity, AuthenticationTypesEnum authenticationType)
    {
        var cleanedClaims = claimsIdentity.Claims.ToList();
        cleanedClaims.RemoveAll(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvetype);
        cleanedClaims.RemoveAll(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveid);
        cleanedClaims.RemoveAll(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvehash);
        cleanedClaims.RemoveAll(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveamr);

        var approveTypeClaim = new Claim(ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvetype, ApproveType.ToString());
        var hashClaim = new Claim(ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approvehash, ChallengeBase64Url);
        var idClaim = new Claim(ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveid, ID.ToString());
        var amrClaim = new Claim(ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveamr, authenticationType.ToString());
        cleanedClaims.Add(approveTypeClaim);
        cleanedClaims.Add(hashClaim);
        cleanedClaims.Add(idClaim);
        cleanedClaims.Add(amrClaim);

        // Create a new identity with the adjusted claims and then persist to the authentication cookie.
        return new ClaimsIdentity(cleanedClaims, authenticationType.ToString(),
            claimsIdentity.NameClaimType, claimsIdentity.RoleClaimType);
    }
}