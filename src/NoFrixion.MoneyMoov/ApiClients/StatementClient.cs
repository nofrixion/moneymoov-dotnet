//-----------------------------------------------------------------------------
// Filename: StatementClient.cs
//
// Description: An API client to call the MoneyMoov Statements API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 25 May 2024  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Net.Http.Headers;

namespace NoFrixion.MoneyMoov;

public interface IStatementClient
{
    Task<RestApiFileResponse> GetStatementAsync(string accessToken, Guid statementID);

    Task<RestApiResponse> ClearStatementsAsync(string accessToken);
}

public class StatementClient : IStatementClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public StatementClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public StatementClient(IRestApiClient apiClient, ILogger<StatementClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov statements endpoint to get a statement file.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="statementID">The ID of the report to retrieve the result for.</param>
    /// <returns>If successful, the statement result.</returns>
    [Obsolete("Use AccountClient.GetStatementAsync instead.")]
    public async Task<RestApiFileResponse> GetStatementAsync(string accessToken, Guid statementID)
    {
        var url = MoneyMoovUrlBuilder.StatementsApi.StatementsUrl(_apiClient.GetBaseUri().ToString(), statementID);
        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetStatementAsync));

        if (!prob.IsEmpty)
        {
            return new RestApiFileResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob);
        }

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/pdf";
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('\"') ?? "downloaded_file";

                return new RestApiFileResponse(HttpStatusCode.OK, new Uri(url), response.Headers, content, contentType, fileName);
            }
            else
            {
                return new RestApiFileResponse(
                    response.StatusCode, 
                    new Uri(url), 
                    new NoFrixionProblem(response.ReasonPhrase ?? "File download failed.", (int)response.StatusCode));
            }
        }
    }

    /// <summary>
    /// Calls the MoneyMoov Statements endpoint to clear the user's cached statements. This allows them to be re-generated, for example
    /// if the statement was for the current month and the month has not yet completed.
    /// </summary>
    /// <param name="accessToken">The user token deleting the payout.</param>
    [Obsolete("Use AccountClient.ClearStatementsAsync instead.")]
    public Task<RestApiResponse> ClearStatementsAsync(string accessToken)
    {
        var url = MoneyMoovUrlBuilder.StatementsApi.StatementsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(ClearStatementsAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.DeleteAsync(url, accessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
