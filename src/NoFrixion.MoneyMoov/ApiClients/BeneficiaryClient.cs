// -----------------------------------------------------------------------------
//  Filename: BeneficiaryClient.cs
// 
//  Description: An API client to call the MoneyMoov Beneficiary API end point.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  08 11 2023  Saurav Maiti Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov;

public interface IBeneficiaryClient
{
    Task<RestApiResponse<Beneficiary>> CreateBeneficiaryAsync(string userAccessToken, BeneficiaryCreate beneficiaryCreate);

    Task<RestApiResponse<Beneficiary>> GetBeneficiaryAsync(string userAccessToken, Guid beneficiaryID);

    Task<RestApiResponse> AuthoriseBeneficiaryAsync(string strongUserAccessToken, Guid beneficiaryID);
}


public class BeneficiaryClient : IBeneficiaryClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public BeneficiaryClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public BeneficiaryClient(IRestApiClient apiClient, ILogger<BeneficiaryClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }
    
    /// <summary>
    /// Calls the MoneyMoov beneficiary endpoint to get a single benefciiary by ID.
    /// </summary>
    /// <param name="userAccessToken">A User scoped JWT access token.</param>
    /// <param name="beneficiaryID">The ID of the beneficiary to retrieve.</param>
    /// <returns>If successful, a beneficiary object.</returns>
    public Task<RestApiResponse<Beneficiary>> GetBeneficiaryAsync(string userAccessToken, Guid beneficiaryID)
    {
        var url = MoneyMoovUrlBuilder.BeneficiariesApi.BeneficiaryUrl(_apiClient.GetBaseUri().ToString(), beneficiaryID);

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(GetBeneficiaryAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<Beneficiary>(url, userAccessToken),
            _ => Task.FromResult(new RestApiResponse<Beneficiary>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
    
    /// <summary>
    /// Calls the MoneyMoov Beneficiary endpoint to create a new Beneficiary. The Beneficiary will be enabled by default but
    /// if the BeneficiaryAuthoriseCount business setting is set then it will be disabled until authorised by the required number of authorisers.
    /// </summary>
    /// <param name="userAccessToken">The access token of the user creating the Beneficiary.</param>
    /// <param name="beneficiaryCreate">A model with the details of the Beneficiary to create.</param>
    /// <returns>An API response indicating the result of the create attempt.</returns>
    public Task<RestApiResponse<Beneficiary>> CreateBeneficiaryAsync(string userAccessToken, BeneficiaryCreate beneficiaryCreate)
    {
        var url = MoneyMoovUrlBuilder.BeneficiariesApi.BeneficiaryUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(userAccessToken, nameof(CreateBeneficiaryAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<Beneficiary>(url, userAccessToken, new FormUrlEncodedContent(beneficiaryCreate.ToDictionary())),
            _ => Task.FromResult(new RestApiResponse<Beneficiary>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
    
    /// <summary>
    /// Calls the MoneyMoov Beneficiary endpoint to authorise an existing Beneficiary to enable it.
    /// </summary>
    /// <param name="strongUserAccessToken">The strong user access token authorised to authorise Beneficiary. Strong
    /// tokens can only be acquired from a strong customer authentication flow, are short lived (typically 5 minute expiry)
    /// and are specific to the Beneficiary being submitted.</param>
    /// <param name="beneficiaryID">The ID of the Beneficiary to authorise.</param>
    /// <returns>An API response indicating the result of the authorise attempt.</returns>
    public Task<RestApiResponse> AuthoriseBeneficiaryAsync(string strongUserAccessToken, Guid beneficiaryID)
    {
        var url = MoneyMoovUrlBuilder.BeneficiariesApi.AuthoriseBeneficiaryUrl(_apiClient.GetBaseUri().ToString(), beneficiaryID);

        var prob = _apiClient.CheckAccessToken(strongUserAccessToken, nameof(AuthoriseBeneficiaryAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync(url, strongUserAccessToken),
            _ => Task.FromResult(new RestApiResponse(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}