﻿//-----------------------------------------------------------------------------
// Filename: MoneyMoovApiClient.cs
//
// Description: An API client used to call MoneyMoov API end points.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using LanguageExt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NoFrixion.MoneyMoov;

public interface IMoneyMoovApiClient
{
    Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path);

    Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path, string accessToken);

    Task<MoneyMoovApiResponse> PostAsync(string path, HttpContent content);

    Task<MoneyMoovApiResponse> PostAsync(string path, string accessToken);

    Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, HttpContent content);

    Task<MoneyMoovApiResponse> PostAsync(string path, string accessToken, HttpContent content);

    Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, string accessToken, HttpContent content);

    Task<MoneyMoovApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content);

    Task<MoneyMoovApiResponse> DeleteAsync(string path, string accessToken);

    Uri GetBaseUri();

    NoFrixionProblem CheckAccessToken(string accessToken, string callerName);
}

public class MoneyMoovApiClient : IMoneyMoovApiClient, IDisposable
{
    public const string HTTP_CLIENT_NAME = "moneymoov";

    private bool _disposed;

    public HttpClient HttpClient { get; set; }

    public MoneyMoovApiClient()
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
    }

    public MoneyMoovApiClient(string baseUri)
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri(baseUri);
    }

    public MoneyMoovApiClient(IHttpClientFactory httpClientFactory)
    {
        HttpClient = httpClientFactory.CreateClient(HTTP_CLIENT_NAME);
    }

    public Uri GetBaseUri()
        => HttpClient.BaseAddress ?? new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);

    public Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path)
        => ExecAsync<T>(BuildRequest(HttpMethod.Get, path, string.Empty, Option<HttpContent>.None));

    public Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path, string accessToken) 
        => ExecAsync<T>(BuildRequest(HttpMethod.Get, path, accessToken, Option<HttpContent>.None));

    public Task<MoneyMoovApiResponse> PostAsync(string path, HttpContent content)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, string.Empty, content));

    public Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, string.Empty, content));

    public Task<MoneyMoovApiResponse> PostAsync(string path, string accessToken, HttpContent content)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, accessToken, content));

    public Task<MoneyMoovApiResponse> PostAsync(string path, string accessToken)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, accessToken, Option<HttpContent>.None));

    public Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, string accessToken, HttpContent content) 
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, accessToken, content));

    public Task<MoneyMoovApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Put, path, accessToken, content));

    public Task<MoneyMoovApiResponse> DeleteAsync(string path, string accessToken)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, accessToken, Option<HttpContent>.None));

    public NoFrixionProblem CheckAccessToken(string accessToken, string callerName)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            return new NoFrixionProblem(HttpStatusCode.PreconditionFailed, $"No access token was available in {callerName}.");
        }
        else
        {
            return NoFrixionProblem.Empty;
        }
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        string accessToken,
        Option<HttpContent> httpContent)
    {
        HttpRequestMessage request = new HttpRequestMessage(method, path);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        if(httpContent.IsSome)
        {
            request.Content = (HttpContent)httpContent;
        }

        return request;
    }

    private async Task<MoneyMoovApiResponse> ToApiResponse(HttpResponseMessage response, Uri? requestUri)
    {
        if (response.IsSuccessStatusCode)
        {
            return new MoneyMoovApiResponse(response.StatusCode, requestUri, response.Headers);
        }
        else if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500 && response.Content.Headers.ContentLength > 0)
        {
            var problemDetails = await response.Content.ReadFromJsonAsync<NoFrixionProblem>();
            return problemDetails != null ?
                new MoneyMoovApiResponse(response.StatusCode, requestUri, response.Headers, problemDetails) :
                new MoneyMoovApiResponse(response.StatusCode, requestUri, response.Headers,
                   new NoFrixionProblem(response.StatusCode, $"Json deserialisation failed for type {typeof(NoFrixionProblem)}."));
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();

            return new MoneyMoovApiResponse(response.StatusCode, requestUri, response.Headers, new NoFrixionProblem(response.StatusCode, error));
        }
    }

    private async Task<MoneyMoovApiResponse<T>> ToApiResponse<T>(HttpResponseMessage response, Uri? requestUri)
    {
        if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength > 0)
        {
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(),
                new Newtonsoft.Json.JsonSerializerSettings
                { 
                    ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace 
                }
            );

            return result != null ?
                new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, result) :
                new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers,
                    new NoFrixionProblem(response.StatusCode, $"Json deserialisation failed for type {typeof(T)}."));
        }
        else if (response.Content.Headers.ContentLength > 0)
        {
            string contentStr = await response.Content.ReadAsStringAsync();

            var problemDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<NoFrixionProblem>(contentStr,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Replace
                });

            if(problemDetails == null)
            {
                problemDetails = new NoFrixionProblem(response.StatusCode, $"Json deserialisation failed for type {typeof(NoFrixionProblem)}.");
            }

            return new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, problemDetails);
        }
        else
        {
            return new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, new NoFrixionProblem(response.StatusCode, string.Empty));
        }
    }

    private async Task<MoneyMoovApiResponse> ExecAsync(
        HttpRequestMessage req,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
        return await ToApiResponse(response, req.RequestUri);
    }

    private async Task<MoneyMoovApiResponse<T>> ExecAsync<T>(
        HttpRequestMessage req,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var response = await HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
        return await ToApiResponse<T>(response, req.RequestUri);
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            HttpClient.Dispose();
            _disposed = true;
        }
    }
}
