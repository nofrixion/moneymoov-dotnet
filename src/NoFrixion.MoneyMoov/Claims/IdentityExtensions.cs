//-----------------------------------------------------------------------------
// Filename: IdentityExtensions.cs
//
// Description: Contains extension methods for the User Identity
//
// Author(s):
// Donal O'Connor (donal@nofrixion.com)
// 
// History:
// 26 Nov 2021  Donal O'Connor  Created, Carmichael House, Dublin, Ireland.
// 27 Sep 2022  Aaron Clauson   Moved from API project to Biz project.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

#nullable disable

using System.Security.Claims;
using System.Security.Principal;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Claims;

public static class IdentityExtensions
{
    public static Guid GetMerchantId(this IIdentity identity)
    {
        var merchantIDClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid)?.Value;

        return merchantIDClaim?.ToGuid() ?? Guid.Empty;
    }

    public static bool MerchantIdExists(this IIdentity identity)
    {
        return identity != null && ((ClaimsIdentity)identity).Claims.Any(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid);
    }

    public static string GetTokenId(this IIdentity identity)
    {
        return ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == NoFrixionClaimsEnum.tokenid.ToString())?.Value;
    }

    public static bool HasAudience(this IIdentity identity, params string[] audiences)
    {
        return audiences.Any(audience => ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == "aud")?.Value == audience);
    }

    public static User GetUser(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        if (claimsIdentity == null || !claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Name))
        {
            return User.Empty;
        }
        else
        {
            return new User
            {
                ID = claimsIdentity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Name)!.Value.ToGuid(),
                FirstName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value,
                LastName = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value,
                EmailAddress = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
            };
        }
    }

    /// <summary>
    /// Attempts to get the UserID from the claim set by the NoFrixion Identity server.
    /// </summary>
    /// <param name="principal">The claims principal wrapping the claims set by the Identity server.</param>
    /// <returns>The User's ID or an empty GUID if it could not be extracted.</returns>
    public static Guid GetMoneyMoovUserID(this ClaimsPrincipal principal)
    {
        var nameClaimValue = principal.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(nameClaimValue))
        {
            return Guid.Empty;
        }
        else
        {
            Guid.TryParse(nameClaimValue, out Guid id);

            return id;
        }
    }

    /// <summary>
    /// Attempts to get the UserID from the claim set by the NoFrixion Identity server.
    /// </summary>
    /// <param name="principal">The claims principal wrapping the claims set by the Identity server.</param>
    /// <returns>The User's ID or an empty GUID if it could not be extracted.</returns>
    public static Guid GetIdentityServerUserID(this ClaimsPrincipal principal)
    {
        var nameIdentifierClaimValue = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(nameIdentifierClaimValue))
        {
            return Guid.Empty;
        }
        else
        {
            nameIdentifierClaimValue = nameIdentifierClaimValue.Replace(ClaimsConstants.NOFRIXION_NAMEID_PREFIX, "");

            Guid.TryParse(nameIdentifierClaimValue, out Guid id);

            return id;
        }
    }

    public static string GetEmailAddress(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        if (claimsIdentity == null)
        {
            return string.Empty;
        }
        else
        {
            return claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }

    public static string GetEmailAddress(this ClaimsPrincipal principal)
    {
        return principal.Identities.First().Claims.SingleOrDefault(x => x.Type == ClaimTypes.Email).Value;
    }

    public static bool IsComplianceOfficer(this IIdentity identity)
        => IsComplianceOfficer(identity as ClaimsIdentity);

    public static bool IsComplianceOfficer(this ClaimsIdentity claimsIdentity)
    {
        if (claimsIdentity == null)
        {
            return false;
        }
        else
        {
            var isComplianceOfficerClaimType = ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.iscompliance;
            return claimsIdentity.Claims.Any(x => x.Type == isComplianceOfficerClaimType && x.Value == Boolean.TrueString);
        }
    }

    public static bool IsUserToken(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        if (claimsIdentity == null)
        {
            return false;
        }
        else
        {
            return !identity.IsComplianceOfficer() && claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Name);
        }
    }

    /// <summary>
    /// Gets a description of the identity of the token owner and/or type. The token
    /// could be a user or merchant token so the description of the owner can vary.
    /// The result of this method is useful for logging, it must NOT be used for any
    /// authorisation logic.
    /// </summary>
    /// <param name="identity">The identity to get the whoami description for.</param>
    /// <returns>A descriptive string for the owner of the token.</returns>
    public static string WhoAmI(this IIdentity identity)
        => WhoAmI(identity as ClaimsIdentity);

    public static string WhoAmI(this ClaimsPrincipal claimsPrincipal)
        => WhoAmI(claimsPrincipal.Identities.First());

    public static string WhoAmI(this ClaimsIdentity claimsIdentity)
    {
        if (claimsIdentity.IsComplianceOfficer())
        {
            return "Compliance " + claimsIdentity.GetEmailAddress();
        }
        else
        {
            if (claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Email))
            {
                return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            }
            else if (claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.GivenName) &&
                claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Surname))
            {
                return
                    claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value + " " +
                    claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value;
            }
            else if (claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Name))
            {
                return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.Name).Value;
            }
            else if (claimsIdentity.Claims.Any(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid))
            {
                return claimsIdentity.Claims.First(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid).Value;
            }
            else
            {
                return "Unknown claims identity";
            }
        }
    }
}

#nullable enable
