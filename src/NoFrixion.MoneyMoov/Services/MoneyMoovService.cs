//-----------------------------------------------------------------------------
// Filename: MoneyMoovService.cs
//
// Description: Service to generate an authorised client and interact with the
// NoFrixion MoneyMoov service.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 Jul 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NoFrixion.MoneyMoov.Services;

public class MoneyMoovService : IMoneyMoovService
{
    private const int MAXIMUM_ERROR_LENGTH = 1024;

    protected readonly ILogger _logger;
    protected readonly IConfiguration _config;
    protected readonly HttpClient _httpClient;
    protected readonly string _moneyMoovApiBaseUrl;

    private string _accessToken;

    public MoneyMoovService(
        ILogger<MoneyMoovService> logger,
        IConfiguration configuration,
        string accessToken, 
        HttpClient httpClient)
    {
        _logger = logger;
        _config = configuration;
        _httpClient = httpClient;
        _accessToken = accessToken;

        _moneyMoovApiBaseUrl = configuration[MoneyMoovConfigKeys.MONEYMOOV_API_BASE_URL];
    }

    protected void PrepareAuthenticatedClient()
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    protected void SetAccessToken(string accessToken)
    {
        _accessToken = accessToken;
    }

    public async Task<Either<NoFrixionProblem, NoFrixionVersion>> VersionAsync()
    {
        PrepareAuthenticatedClient();
        var url = MoneyMoovUrlBuilder.VersionUrl(_moneyMoovApiBaseUrl);
        var response = await _httpClient.GetAsync(url);
        return await FromResponse<NoFrixionVersion>(response);     
    }

    public async Task<Either<NoFrixionProblem, User>> WhoamiAsync()
    {
        PrepareAuthenticatedClient();
        var url = MoneyMoovUrlBuilder.WhoamiUrl(_moneyMoovApiBaseUrl);
        var response = await _httpClient.GetAsync(url);
        return await FromResponse<User>(response);
    }

    protected async Task<Either<NoFrixionProblem, T>> FromResponse<T>(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            var version = await response.Content.ReadFromJsonAsync<T>();
            return version != null ? version :
                new NoFrixionProblem($"The MoneyMoov {typeof(T).Name} details could not be deserialised in {nameof(MoneyMoovService)}.");
        }
        else if(response.Content != null )
        {
            if (response.Content.Headers.ContentType?.MediaType == "application/json")
            {
                return await response.Content.ReadFromJsonAsync<NoFrixionProblem>() ??
                    new NoFrixionProblem($"The MoneyMoov problem details could not be deserialised in {nameof(MoneyMoovService)}.");
            }
            else
            {
                string? error = await response.Content.ReadAsStringAsync();
                error = error?.Length > MAXIMUM_ERROR_LENGTH ? error.Substring(MAXIMUM_ERROR_LENGTH) : error;
                return new NoFrixionProblem($"An error response of {response.StatusCode} was received from the MoneyMoov API in {nameof(MoneyMoovService)}. {error}");
            }
        }
        else
        {
            return new NoFrixionProblem($"An error response of {response.StatusCode} was received from the MoneyMoov API.");
        }
    }
}
