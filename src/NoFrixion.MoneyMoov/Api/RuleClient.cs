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

namespace NoFrixion.MoneyMoov;

public interface IRuleClient
{
    Task<MoneyMoovApiResponse> ApproveRuleAsync(string userAccessStrongToken, Guid ruleID);
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
}
