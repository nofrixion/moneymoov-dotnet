//-----------------------------------------------------------------------------
// Filename: RuleClient.cs
//
// Description: An API client to call the MoneyMoov Rule API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 28 Jan 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IRuleClient
{
    Task<MoneyMoovApiResponse> ApproveRuleAsync(string userAccessStrongToken, Guid ruleID);

    Task<MoneyMoovApiResponse<Rule>> GetRuleAsync(string userAccessToken, Guid ruleID);
}

public class RuleClient : IRuleClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public RuleClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public RuleClient(IMoneyMoovApiClient apiClient, ILogger<RuleClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Rule endpoint to approve a rule for execution.
    /// </summary>
    /// <param name="userAccessStrongToken">A User scoped JWT strong access token.</param>
    /// <param name="ruleID">The ID of the rule being authorised, must match the token.</param>
    /// <returns>An API response indicating the result of the approval attempt.</returns>
    public Task<MoneyMoovApiResponse> ApproveRuleAsync(string userAccessStrongToken, Guid ruleID)
    {
        var url = MoneyMoovUrlBuilder.RulesApi.ApproveRuleUrl(_apiClient.GetBaseUri().ToString(), ruleID);
        return _apiClient.PostAsync(url, userAccessStrongToken);
    }

    /// <summary>
    /// Calls the MoneyMoov rule endpoint to get a single Rule by ID.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="ruleID">The ID of the rule to retrieve.</param>
    /// <returns>If successful, a rule object.</returns>
    public Task<MoneyMoovApiResponse<Rule>> GetRuleAsync(string userAccessToken, Guid ruleID)
    {
        var url = MoneyMoovUrlBuilder.RulesApi.RuleUrl(_apiClient.GetBaseUri().ToString(), ruleID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetRuleAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<Rule>(url, userAccessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<Rule>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
