//-----------------------------------------------------------------------------
// Filename: MerchsntClient.cs
//
// Description: An API client to call the MoneyMoov Merchant API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 27 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public class MerchantClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public MerchantClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public MerchantClient(IMoneyMoovApiClient apiClient, ILogger<MetadataClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov UserRoles Merchant endpoint to get the list of users who have been
    /// assigned a role on the merchant.
    /// </summary>
    /// <param name="auserAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the user roles for.</param>
    /// <returns>If successful, a list of the user role assignments for the merchant.</returns>
    public Task<MoneyMoovApiResponse<IEnumerable<UserRole>>> GetUserRolesAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.UserRolesApiUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<IEnumerable<UserRole>>(url, userAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<IEnumerable<UserRole>>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get merchant tokens endpoint to get the list of all the merchant tokens
    /// that have been allocated for the merchant.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the merchant tokens for.</param>
    /// <returns>If successful, a list of tokens for the merchant.</returns>
    public Task<MoneyMoovApiResponse<MerchantTokenPageResponse>> GetMerchantTokensAsync(string userAccessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.MerchantGetTokensApiUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<MerchantTokenPageResponse>(url, userAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<MerchantTokenPageResponse>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant create merchant token endpoint to create anew merchant scoped token.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="token">The detaals of the token to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<MoneyMoovApiResponse<MerchantToken>> CreateMerchantTokenAsync(string userAccessToken, TokenAdd token)
    {
        var url = MoneyMoovUrlBuilder.UserMerchantTokensApiUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetUserRolesAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<MerchantToken>(url, userAccessToken, new FormUrlEncodedContent(token.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<MerchantToken>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
