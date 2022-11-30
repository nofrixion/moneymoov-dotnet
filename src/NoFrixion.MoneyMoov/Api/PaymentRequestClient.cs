//-----------------------------------------------------------------------------
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
    Task<MoneyMoovApiResponse<PaymentRequest>> GetPaymentRequestAsync(string accessToken, Guid paymentRequestID);

    Task<MoneyMoovApiResponse<PaymentRequest>> CreatePaymentRequestAsync(string accessToken, PaymentRequestCreate paymentRequestCreate);
}

public class PaymentRequestClient : IPaymentRequestClient
{
    private readonly ILogger _logger;
    private readonly IMoneyMoovApiClient _apiClient;

    public PaymentRequestClient(IMoneyMoovApiClient apiClient)
    {
        _apiClient = apiClient;
        _logger = NullLogger.Instance;
    }

    public PaymentRequestClient(IMoneyMoovApiClient apiClient, ILogger<PaymentRequestClient> logger)
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
    public Task<MoneyMoovApiResponse<PaymentRequest>> GetPaymentRequestAsync(string accessToken, Guid paymentRequestID)
    {
        var url = MoneyMoovUrlBuilder.PaymentRequestsApi.GetByIDUrl(_apiClient.GetBaseUri().ToString(), paymentRequestID);

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(GetPaymentRequestAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.GetAsync<PaymentRequest>(url, accessToken),
            _ => Task.FromResult(new MoneyMoovApiResponse<PaymentRequest>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }

    /// <summary>
    /// Calls the MoneyMoov Merchant create payment request endpoint to create a new payment request.
    /// </summary>
    /// <param name="accessToken">A User or Merchant scoped JWT access token.</param>
    /// <param name="paymentRequestCreate">The details of the payment request to create.</param>
    /// <returns>If successful, the newly created merchant token.</returns>
    public Task<MoneyMoovApiResponse<PaymentRequest>> CreatePaymentRequestAsync(string accessToken, PaymentRequestCreate paymentRequestCreate)
    {
        var url = MoneyMoovUrlBuilder.PaymentRequestsApi.CreatePaymentRequestUrl(_apiClient.GetBaseUri().ToString());

        var prob = _apiClient.CheckAccessToken(accessToken, nameof(CreatePaymentRequestAsync));

        return prob switch
        {
            var p when p.IsEmpty => _apiClient.PostAsync<PaymentRequest>(url, accessToken, new FormUrlEncodedContent(paymentRequestCreate.ToDictionary())),
            _ => Task.FromResult(new MoneyMoovApiResponse<PaymentRequest>(HttpStatusCode.PreconditionFailed, new Uri(url), prob))
        };
    }
}
