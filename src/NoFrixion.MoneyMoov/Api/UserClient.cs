//-----------------------------------------------------------------------------
// Filename: UserClient.cs
//
// Description: An API client to call the MoneyMoov User API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 13 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IUserClient
{
    Task<MoneyMoovApiResponse> SendInviteAsync(string userAccessToken, Guid merchantID, string inviteeEmailAddress);
}

public class UserClient : IUserClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public UserClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public UserClient(IMoneyMoovApiClient apiClient, ILogger<AccountClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov account endpoint to create and send an email invite to register
    /// and join a merchant account.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="merchantID">The ID of the merchant the user is being invited to join.</param>
    /// <param name="inviteeEmailAddress">The email address of the user to invite.</param>
    public Task<MoneyMoovApiResponse> SendInviteAsync(string userAccessToken, Guid merchantID, string inviteeEmailAddress)
    {
        var url = MoneyMoovUrlBuilder.UserApi.SendInviteApiUrl(_apiClient.GetBaseUri().ToString(), merchantID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(SendInviteAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync(url, userAccessToken, new FormUrlEncodedContent(new Dictionary<string,string> { { "inviteeEmailAddress", inviteeEmailAddress } })),
            _ => Task.FromResult(new MoneyMoovApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
