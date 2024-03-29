﻿//-----------------------------------------------------------------------------
// Filename: PaymentRequestClient.cs
//
// Description: An API client to call the MoneyMoov Payment Request API end point.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 29 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NoFrixion.MoneyMoov.Models;
using System.Net;

namespace NoFrixion.MoneyMoov;

public interface IPaymentRequestClient
{
    Task<RestApiResponse<PaymentRequest>> GetPaymentRequestAsync(string accessToken, Guid paymentRequestID);

    Task<RestApiResponse<PaymentRequest>> GetPaymentRequestByOrderAsync(string accessToken, string orderID);

    Task<RestApiResponse<PaymentRequest>> CreatePaymentRequestAsync(string accessToken, PaymentRequestCreate paymentRequestCreate);

    //Task<RestApiResponse> DeletePaymentRequestAsync(string accessToken, Guid paymentRequestID);
}

public class PaymentRequestClient : IPaymentRequestClient
{
    private readonly ILogger _logger;
    private readonly IRestApiClient _apiClient;

    public PaymentRequestClient()
    {
        _apiClient = new RestApiClient(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        _logger = NullLogger.Instance;
    }

    public PaymentRequestClient(string baseUri)
    {
        _apiClient = new RestApiClient(baseUri);
        _logger = NullLogger.Instance;
    }

    public PaymentRequestClient(IRestApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public PaymentRequestClient(IRestApiClient apiClient, ILogger<PaymentRequestClient> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get payment request endpoint to get a single payment request by ID.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="paymentRequestID">The ID of the payment request to retrieve.</param>
    /// <returns>If successful, a payment request object.</returns>
    public Task<RestApiResponse<PaymentRequest>> GetPaymentRequestAsync(string accessToken, Guid paymentRequestID)
    {
        var url = MoneyMoovUrlBuilder.PaymentRequestsApi.PaymentRequestUrl(_apiClient.GetBaseUri().ToString(), paymentRequestID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetPaymentRequestAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<PaymentRequest>(url, accessToken),
            _ => Task.FromResult(new RestApiResponse<PaymentRequest>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant get payment request endpoint to get a single payment request by merchant ID and order ID.
    /// </summary>
    /// <param name="accessToken">A Merchant scoped JWT access token. NOte must be a merchant token as a merchant ID
    /// is required when retrieving a payment request by order.</param>
    /// <param name="orderID">The order ID of the payment request to retrieve.</param>
    /// <returns>If successful, a payment request object.</returns>
    public Task<RestApiResponse<PaymentRequest>> GetPaymentRequestByOrderAsync(string accessToken, string orderID)
    {
        var url = MoneyMoovUrlBuilder.PaymentRequestsApi.GetByOrderIDUrl(_apiClient.GetBaseUri().ToString(), orderID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetPaymentRequestAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<PaymentRequest>(url, accessToken),
            _ => Task.FromResult(new RestApiResponse<PaymentRequest>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant create payment request endpoint to create a new payment request.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="paymentRequestCreate">The details of the payment request to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<RestApiResponse<PaymentRequest>> CreatePaymentRequestAsync(string accessToken, PaymentRequestCreate paymentRequestCreate)
    {
        var url = MoneyMoovUrlBuilder.PaymentRequestsApi.PaymentRequestsUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(CreatePaymentRequestAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<PaymentRequest>(url, accessToken, new FormUrlEncodedContent(paymentRequestCreate.ToDictionary())),
            _ => Task.FromResult(new RestApiResponse<PaymentRequest>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
