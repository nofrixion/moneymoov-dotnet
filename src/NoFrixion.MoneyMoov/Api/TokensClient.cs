//-----------------------------------------------------------------------------
// Filename: TokensClient.cs
//
// Description: An API client to call the MoneyMoov Tokens API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface ITokensClient
{
    Task<MoneyMoovApiResponse> DeleteTokenAsync(string accessToken, Guid id);
}

public class TokensClient : ITokensClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public TokensClient()
    {
        _apiClient = new MoneyMoovApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public TokensClient(string baseUri)
    {
        _apiClient = new MoneyMoovApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public TokensClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public TokensClient(IMoneyMoovApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Deletes a user or merchant token.
    /// </summary>
    /// <param name="accessToken">A User scoped JWT access token.</param>
    /// <param name="id">The ID of the token to delete.</param>
    /// <returns>A moneymoov API response object.</returns>
    public Task<MoneyMoovApiResponse> DeleteTokenAsync(string accessToken, Guid id)
    {
        var url = MoneyMoovUrlBuilder.TokensApi.TokenUrl(_apiClient.GetBaseUri().ToString(), id);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(DeleteTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, accessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
