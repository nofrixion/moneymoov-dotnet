// -----------------------------------------------------------------------------
//  Filename: NoFrixionIdentityTokenAcquirer.cs
// 
//  Description: Contains the logic to acquire newly minted OAuth tokens from NoFrixion Identity.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  28 07 2022  Donal O'Connor   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.Text;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using NoFrixion.Biz.TokenAcquirers;
using NoFrixion.MoneyMoov.Constants;
using NoFrixion.MoneyMoov.Models.Tokens;
using AuthorizationCodeTokenRequest = IdentityModel.Client.AuthorizationCodeTokenRequest;
using RefreshTokenRequest = IdentityModel.Client.RefreshTokenRequest;

namespace NoFrixion.MoneyMoov.TokenAcquirers;

public class NoFrixionTokenAcquirer : ITokenAcquirer
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public NoFrixionTokenAcquirer(IHttpClientFactory httpClientFactory, 
        IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    /// <summary>
    /// Gets a redirect URL to request the current user complete a multi-factor authentication
    /// step to acquire a NoFrixion Identity "code" (a short, single use, time limited string) that can be
    /// sent back to NoFrixion Identity to get a JWT identity and/or access token.
    /// </summary>
    /// <param name="callbackUrl">The URL NoFrixion Identity will redirect the use to after a successful
    /// authentication.</param>
    /// <param name="state">A pseudo-random state field that NoFrixion Identity will include in the redirect
    /// response to provide replay protection.</param>
    /// <param name="tokenType">The type of token to attempt to generate.</param>
    /// <param name="customClaims">A dictionary of custom claims that should be added to the token.</param>
    /// <returns>A URL the human user needs to be redirected to in order to initiate the token
    /// issuance.</returns>
    public string GetLoginRedirectUrl(string callbackUrl, string state, TokenTypesEnum tokenType, Dictionary<string, string>? customClaims = null)
    {
        // The scope determines which audience is returned by NoFrixion Identity.
        var nofrixionScope = tokenType switch
        {
            TokenTypesEnum.Machine => TokenConstants.NOFRIXION_STANDARD_SCOPE,
            TokenTypesEnum.Standard => TokenConstants.NOFRIXION_STANDARD_SCOPE,
            TokenTypesEnum.Strong => TokenConstants.NOFRIXION_STRONG_SCOPE,
            TokenTypesEnum.UserApiToken => TokenConstants.NOFRIXION_STANDARD_SCOPE,
            _ => _config[IdentityConfigKeys.IDENTITY_SERVER_AUDIENCE]
        };

        var scopes = tokenType switch
        {
            TokenTypesEnum.Machine => TokenConstants.CREATE_MACHINE_TOKEN_SCOPES,
            TokenTypesEnum.Standard => TokenConstants.CREATE_STANDARD_TOKEN_SCOPES,
            TokenTypesEnum.Strong => TokenConstants.CREATE_STRONG_TOKEN_SCOPES,
            TokenTypesEnum.UserApiToken => TokenConstants.CREATE_MACHINE_TOKEN_SCOPES,
            _ => TokenConstants.CREATE_STANDARD_TOKEN_SCOPES
        };

        // Add nofrixion scope to get the correct audience
        scopes = $"{scopes} {nofrixionScope}";

        if (customClaims != null)
        {
            foreach (var customClaim in customClaims)
            {
                scopes += $" {customClaim.Key}:{customClaim.Value}";
            }
        }

        var urlBuilder = new StringBuilder();

        urlBuilder.Append($"{_config[IdentityConfigKeys.NOFRIXION_IDENTITY_DOMAIN]}/connect/authorize");
        urlBuilder.Append($"?client_id={_config[IdentityConfigKeys.NOFRIXION_IDENTITY_CLIENTID]}");
        urlBuilder.Append($"&scope={System.Uri.EscapeDataString(scopes)}");
        urlBuilder.Append("&response_type=code");
        urlBuilder.Append($"&state={state}");
        urlBuilder.Append($"&acr_values=mfa"); // Used to trigger mfa.
        urlBuilder.Append($"&redirect_uri={callbackUrl}");

        return urlBuilder.ToString();
    }

    /// <summary>
    /// Once a user successfully completes the MFA check with NoFrixion Identity they will be issued with
    /// a code (a short, single use, time limited string) that can be sent to the NoFrixion Identity token
    /// REST endpoint to get a JWT identity and/or access and/or refresh token. This method
    /// takes care of contacting the NoFrixion Identity token REST end point.
    /// </summary>
    /// <param name="code">The code returned by NoFrixion Identity when the user successfully completed their MFA 
    /// authentication step.</param>
    /// <param name="redirectUrl">A URL that must be sent to NoFrixion Identity in the get token request but
    /// doesn't actually seem to get used.</param>
    /// <returns>A token response which, if successful, contains the issued JWT tokens.</returns>
    public async Task<AccessTokenResponse> GetToken(string code, string redirectUrl)
    {
        var client = _httpClientFactory.CreateClient(HttpClientConstants.HTTP_NOFRIXION_IDENTITY_CLIENT_NAME);

        var tokenUrl = $"{_config[IdentityConfigKeys.NOFRIXION_IDENTITY_DOMAIN]}/connect/token";

        var tokenResponse = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
        {
            Address = tokenUrl,

            ClientId = _config[IdentityConfigKeys.NOFRIXION_IDENTITY_CLIENTID]!,
            ClientSecret = _config[IdentityConfigKeys.NOFRIXION_IDENTITY_CLIENTSECRET],

            Code = code,

            RedirectUri = redirectUrl,
        });

        return new AccessTokenResponse
        {
            AccessToken = tokenResponse.AccessToken, 
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
        };
    }

    public async Task<AccessTokenResponse> RefreshToken(string refreshToken)
    {
        var client = _httpClientFactory.CreateClient(HttpClientConstants.HTTP_NOFRIXION_IDENTITY_CLIENT_NAME);

        var tokenUrl = $"{_config[IdentityConfigKeys.NOFRIXION_IDENTITY_DOMAIN]}/connect/token";

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
        {
            Address = tokenUrl,

            ClientId = _config[IdentityConfigKeys.NOFRIXION_IDENTITY_CLIENTID]!,
            ClientSecret = _config[IdentityConfigKeys.NOFRIXION_IDENTITY_CLIENTSECRET],

            RefreshToken = refreshToken,
        });

        return new AccessTokenResponse
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = tokenResponse.ExpiresIn,
        };
    }
}