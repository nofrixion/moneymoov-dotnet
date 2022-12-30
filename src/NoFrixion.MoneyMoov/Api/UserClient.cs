//-----------------------------------------------------------------------------
// Filename: UserClient.cs
//
// Description: An API client to call the MoneyMoov User API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Dec 2022  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov;

public interface IUserClient
{
    Task<MoneyMoovApiResponse<User>> CreateUserAsync(UserCreate userCreate);
}

public class UserClient : IUserClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public UserClient()
    {
        _apiClient = new MoneyMoovApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public UserClient(string baseUri)
    {
        _apiClient = new MoneyMoovApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

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
    /// Calls the MoneyMoov Merchant get user endpoint create a new user record.
    /// </summary>
    /// <param name="userCreate">The model containing the data about the new user to create.</param>
    /// <returns>If successful, a user object.</returns>
    public Task<MoneyMoovApiResponse<User>> CreateUserAsync(UserCreate userCreate)
    {
        var url = MoneyMoovUrlBuilder.UserApi.UserApiUrl(_apiClient.GetBaseUri().ToString());
        return _apiClient.PostAsync<User>(url, new FormUrlEncodedContent(userCreate.ToDictionary()));
    } 
}
