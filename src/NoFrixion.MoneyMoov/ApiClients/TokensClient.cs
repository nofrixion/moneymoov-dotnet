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
    [Obsolete($"Use {nameof(ITokensClient)}.{nameof(DeleteTokenAsync)} instead.")]
    Task<RestApiResponse> DeleteTokenAsync(string accessToken, Guid id);
    
    Task<RestApiResponse> ArchiveTokenAsync(string accessToken, Guid id);

    Task<RestApiResponse> AuthoriseTokenAsync(string accessToken, Guid id);
}

public class TokensClient : ITokensClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public TokensClient()
    {
        _apiClient = new RestApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public TokensClient(string baseUri)
    {
        _apiClient = new RestApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public TokensClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public TokensClient(IRestApiClient apiClient, ILogger<AccountClient> logger)
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
    [Obsolete($"Use {nameof(TokensClient)}.{nameof(ArchiveTokenAsync)} instead.")]
    public Task<RestApiResponse> DeleteTokenAsync(string accessToken, Guid id)
    {
        return ArchiveTokenAsync(accessToken, id);
    }

    /// <summary>
    /// Archives a user or merchant token.
    /// </summary>
    /// <param name="accessToken">A User scoped JWT access token.</param>
    /// <param name="id">The ID of the token to archive.</param>
    /// <returns>A moneymoov API response object.</returns>
    public Task<RestApiResponse> ArchiveTokenAsync(string accessToken, Guid id)
    {
        var url = MoneyMoovUrlBuilder.TokensApi.TokenUrl(_apiClient.GetBaseUri().ToString(), id);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(ArchiveTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, accessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    public Task<RestApiResponse> AuthoriseTokenAsync(string accessToken, Guid id)
    {
        var url = MoneyMoovUrlBuilder.TokensApi.AuthoriseTokenUrl(_apiClient.GetBaseUri().ToString(), id);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(AuthoriseTokenAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync(url, accessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
