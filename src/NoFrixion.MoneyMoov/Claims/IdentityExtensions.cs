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
using NoFrixion.Common.Permissions;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Extensions;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Claims;

public static class IdentityExtensions
{
    public static bool TwoFactorEnabled(this IIdentity identity)
    {
        var twoFactorEnabledClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == NoFrixionClaimsEnum.two_factor_enabled.ToString())?.Value;

        return bool.TryParse(twoFactorEnabledClaim, out var twoFactorEnabled) && twoFactorEnabled;
    }
    
    public static bool PasskeyAdded(this IIdentity identity)
    {
        var passkeyClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == NoFrixionClaimsEnum.passkey_added.ToString())?.Value;

        return bool.TryParse(passkeyClaim, out var passkeyEnabled) && passkeyEnabled;
    }
    
    public static Guid GetMerchantId(this IIdentity identity)
    {
        var merchantIDClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid)?.Value;

        return merchantIDClaim?.ToGuid() ?? Guid.Empty;
    }

    public static string ApiAppId(this IIdentity identity)
    {
        var verifiedClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.appid)?.Value;

        return verifiedClaim ?? string.Empty;
    }
    
    public static bool IsVerfiedByApiKey(this IIdentity identity)
    {
        var verifiedClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.verfied_by_api_key)?.Value;

        return bool.TryParse(verifiedClaim, out var verifiedByApiKey) && verifiedByApiKey;
    }

    /// <summary>
    /// Returns true if the request was authenticated with a merchant JWT bearer token.
    /// </summary>
    public static bool IsMerchantTokenBearer(this IIdentity identity)
    {
        var verifiedClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchant_token_bearer)?.Value;

        return bool.TryParse(verifiedClaim, out var verifiedMerchantToken) && verifiedMerchantToken;
    }

    /// <summary>
    /// Returns true if a merchant token authenticated request was from a whitelisted source IP address.
    /// </summary>
    public static bool IsMerchantTokenIPAddressWhiteLised(this IIdentity identity)
    {
        var ipAddressWhitelistClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchant_token_whitelisted_ipaddress)?.Value;

        return bool.TryParse(ipAddressWhitelistClaim, out var isRquestIPAddressWhiteListed) && isRquestIPAddressWhiteListed;
    }

    /// <summary>
    /// Returns true if a merchant token authenticated request was authenticated with an HMAC or public key signature.
    /// </summary>
    public static bool IsMerchantTokenSigned(this IIdentity identity)
    {
        var isSignedClaim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchant_token_signed)?.Value;

        return bool.TryParse(isSignedClaim, out var isSigned) && isSigned;
    }

    public static bool MerchantIdExists(this IIdentity identity)
    {
        return identity != null && ((ClaimsIdentity)identity).Claims.Any(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.merchantid);
    }

    public static string GetTokenId(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        if(claimsIdentity == null)
        {
            return null;
        }

        return claimsIdentity.FindFirst(x => x.Type == NoFrixionClaimsEnum.tokenid.ToString())?.Value ??
            claimsIdentity?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.tokenid.ToString())?.Value;
    }

    public static bool HasAudience(this IIdentity identity, params string[] audiences)
    {
        return audiences
            .Where(a => !string.IsNullOrEmpty(a))
            .Any(audience => ((ClaimsIdentity)identity)?
                .FindFirst(x => x.Type == "aud")?.Value == audience);
    }

    public static bool IsUsingStandardUserRoles(this IIdentity identity)
    {
        var claim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.use_standard_user_roles)?.Value;

        return bool.TryParse(claim, out var exists) && exists;
    }
    
    public static bool IsUsingPermissions(this IIdentity identity)
    {
        var claim = ((ClaimsIdentity)identity)?.FindFirst(x => x.Type == ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.use_permissions)?.Value;

        return bool.TryParse(claim, out var exists) && exists;
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
            var subClaimValue = principal.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(subClaimValue))
            {
                return Guid.Empty;
            }
            else
            {
                subClaimValue = subClaimValue.Replace(ClaimsConstants.NOFRIXION_NAMEID_PREFIX, "");

                if (Guid.TryParse(subClaimValue, out Guid id))
                {
                    return id;
                }
                else
                {
                    return Guid.Empty;
                }
            }
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

    public static bool IsOperationsOfficer(this IIdentity identity)
        => IsOperationsOfficer(identity as ClaimsIdentity);

    public static bool IsNoFrixionOfficer(this IIdentity identity)
        => IsComplianceOfficer(identity as ClaimsIdentity) || IsOperationsOfficer(identity as ClaimsIdentity);

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

    public static bool IsOperationsOfficer(this ClaimsIdentity claimsIdentity)
    {
        if (claimsIdentity == null)
        {
            return false;
        }
        else
        {
            var isOperationsOfficerClaimType = ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.isoperations;
            return claimsIdentity.Claims.Any(x => x.Type == isOperationsOfficerClaimType && x.Value == Boolean.TrueString);
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
            return !identity.IsNoFrixionOfficer() && claimsIdentity.Claims.Any(x => x.Type == ClaimTypes.Name);
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
        if(claimsIdentity == null)
        {
            return "Empty claims identity";
        }

        if (claimsIdentity.IsComplianceOfficer())
        {
            return "Compliance " + claimsIdentity.GetEmailAddress();
        }

        if (claimsIdentity.IsOperationsOfficer())
        {
            return "Operations " + claimsIdentity.GetEmailAddress();
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

    /// <summary>
    /// Returns true if the identity has the specified permission for the merchant.
    /// Else returns false.
    /// </summary>
    /// <param name="identity">The token identity</param>
    /// <param name="permission">The permissions to be searched in claims.</param>
    /// <param name="merchantID">The ID of the merchant to look permissions for.</param>
    /// <returns></returns>
    public static bool HasMerchantPermission(this IIdentity identity, MerchantPermissions permission, Guid merchantID)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
        {
            return false;
        }

        var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == $"{ClaimTypePrefixes.MERCHANT}.{merchantID}");
        
        if (claim == null)
        {
            return false;
        }
        
        return Enum.TryParse(claim.Value, out MerchantPermissions claimPermissions) && claimPermissions.HasFlag(permission);
    }

    public static bool HasAccountPermission(this IIdentity identity, AccountPermissions permission, Guid accountID)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
        {
            return false;
        }

        var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == $"{ClaimTypePrefixes.ACCOUNT}.{accountID}");
        
        if (claim == null)
        {
            return false;
        }
        
        return Enum.TryParse(claim.Value, out AccountPermissions claimPermissions) && claimPermissions.HasFlag(permission);
    }
    
    /// <summary>
    /// Gets the authentication type from the identity token.
    /// </summary>
    /// <param name="identity">The token identity</param>
    /// <returns>The authentication type.</returns>
    public static AuthenticationTypesEnum GetAuthenticationType(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;

        if (claimsIdentity == null) 
        {
            return AuthenticationTypesEnum.None;
        }
        else
        {
            var authenticationClaimType = ClaimsConstants.NOFRIXION_CLAIMS_NAMESPACE + NoFrixionClaimsEnum.approveamr;

            var authenticationTypeClaimValue = claimsIdentity.Claims.FirstOrDefault(x => x.Type == authenticationClaimType)?.Value;

            if (Enum.TryParse(authenticationTypeClaimValue, out AuthenticationTypesEnum authenticationType))
            {
                return authenticationType;
            }
            else
            {
                return AuthenticationTypesEnum.None;
            }
        }
    }
    
    public static bool HasPermissionsForMerchant(this IIdentity identity, Guid merchantID)
    {
        if (identity is not ClaimsIdentity claimsIdentity)
        {
            return false;
        }

        var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == $"{ClaimTypePrefixes.MERCHANT}.{merchantID}");
        
        return claim != null;
    }
}

#nullable enable
