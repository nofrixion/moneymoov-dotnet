//-----------------------------------------------------------------------------
// Filename: ReportClient.cs
//
// Description: An API client to call the MoneyMoov Report API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 16 Jan 2024  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IReportClient
{
    Task<RestApiResponse<ReportResult>> GetReportResultAsync(string accessToken, Guid reportID, int? statementNumber);
}

public class ReportClient : IReportClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public ReportClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public ReportClient(IRestApiClient apiClient, ILogger<ReportClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov report endpoint to get the results of a report execution.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="reportID">The ID of the report to retrieve the result for.</param>
    /// <param name="statementNumber">An optional statement number to get the specific result for.
    /// If not specified the most recent result is retrieved.</param>
    /// <returns>If successful, the report result.</returns>
    public Task<RestApiResponse<ReportResult>> GetReportResultAsync(string accessToken, Guid reportID, int? statementNumber)
    {
        var url = MoneyMoovUrlBuilder.ReportsApi.ReportResultUrl(_apiClient.GetBaseUri().ToString(), reportID, statementNumber);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetReportResultAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<ReportResult>(url, accessToken),
            _ => Task.FromResult(new RestApiResponse<ReportResult>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
