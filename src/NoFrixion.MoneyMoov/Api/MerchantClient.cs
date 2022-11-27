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

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;
using System.Net.Http.Json;

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
    /// Calls the MoneyMoov UserRoles Merchant endpoint to get the list of uses who have been
    /// assigned a role on the merchant..
    /// </summary>
    /// <param name="accessToken">A Merchant JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant to get the user roles for.</param>
    /// <returns>If successful a list of the user role assignments for the merchant.</returns>
    public Task<MoneyMoovApiResponse<UserRole>> GetUserRolesAsync(string accessToken, Guid merchantID)
    {
        var url = MoneyMoovUrlBuilder.UserRolesApiUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetUserRolesAsync));

        return !prob.IsEmpty ?
            Task.FromResult(new MoneyMoovApiResponse<UserRole>(HttpStatusCode.PreconditionFailed, new Uri(url), prob)) :
            _apiClient.GetAsync<UserRole>(url, accessToken);
    }
}
