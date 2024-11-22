//-----------------------------------------------------------------------------
// Filename: RestApiClient.cs
//
// Description: A REST API client.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 25 OCt 2024  Aaron Clauson   Added HMAC signature authentication overloads.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using LanguageExt;
using System.Net;
using System.Net.Http.Headers;

namespace NoFrixion.MoneyMoov;

public interface IRestApiClient
{
    Task<RestApiResponse<T>> GetAsync<T>(string path);

    Task<RestApiResponse<T>> GetAsync<T>(string path, string accessToken);

    Task<RestApiResponse<T>> GetAsync<T>(string path, Guid appID, string secret, Guid merchantID);

    Task<RestApiResponse<T>> PostAsync<T>(string path);

    Task<RestApiResponse> PostAsync(string path, HttpContent content);

    Task<RestApiResponse> PostAsync(string path, string accessToken);

    Task<RestApiResponse> PostAsync(string path, Guid appID, string secret, Guid merchantID);

    Task<RestApiResponse<T>> PostAsync<T>(string path, HttpContent content);

    Task<RestApiResponse<T>> PostAsync<T>(string path, HttpContent content, Dictionary<string, string> headers,
        MediaTypeWithQualityHeaderValue acceptHeader);

    Task<RestApiResponse> PostAsync(string path, string accessToken, HttpContent content);

    Task<RestApiResponse> PostAsync(string path, Guid appID, string secret, Guid merchantID, HttpContent content);

    Task<RestApiResponse<T>> PostAsync<T>(string path, string accessToken, HttpContent content);

    Task<RestApiResponse<T>> PostAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content);

    Task<RestApiResponse> PutAsync(string path);

    Task<RestApiResponse> PutAsync(string path, string accessToken);

    Task<RestApiResponse> PutAsync(string path, Guid appID, string secret, Guid merchantID);

    Task<RestApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content);

    Task<RestApiResponse<T>> PutAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content);

    Task<RestApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content, string rowVersion);

    Task<RestApiResponse<T>> PutAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content, string rowVersion);

    Task<RestApiResponse> DeleteAsync(string path);

    Task<RestApiResponse> DeleteAsync(string path, string accessToken);

    Task<RestApiResponse> DeleteAsync(string path, Guid appID, string secret, Guid merchantID);

    Task<RestApiResponse> DeleteAsync(string path, string accessToken, string rowVersion);

    Task<RestApiResponse> DeleteAsync(string path, Guid appID, string secret, Guid merchantID, string rowVersion);

    Uri GetBaseUri();

    NoFrixionProblem CheckAccessToken(string accessToken, string callerName);
}

public class RestApiClient : IRestApiClient, IDisposable
{
    private bool _disposed;

    public HttpClient HttpClient { get; set; }
    
    private readonly bool _dataExpectedOnErrorResponse;

    public RestApiClient(string baseUri)
    {
        HttpClient = new HttpClient();
        HttpClient.BaseAddress = new Uri(baseUri);
    }

    public RestApiClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }

    public RestApiClient(IHttpClientFactory httpClientFactory, string httpClientName)
    {
        HttpClient = httpClientFactory.CreateClient(httpClientName);
    }
    
    public RestApiClient(HttpClient httpClient, bool dataExpectedOnErrorResponse)
    {
        HttpClient = httpClient;
        _dataExpectedOnErrorResponse = dataExpectedOnErrorResponse;
    }

    public Uri GetBaseUri()
        => HttpClient.BaseAddress ?? new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);

    public Task<RestApiResponse<T>> GetAsync<T>(string path)
        => ExecAsync<T>(BuildRequest(HttpMethod.Get, path, string.Empty, Option<HttpContent>.None));

    public Task<RestApiResponse<T>> GetAsync<T>(string path, string accessToken) 
        => ExecAsync<T>(BuildRequest(HttpMethod.Get, path, accessToken, Option<HttpContent>.None));

    public Task<RestApiResponse<T>> GetAsync<T>(string path, Guid appID, string secret, Guid merchantID)
        => ExecAsync<T>(BuildRequest(HttpMethod.Get, path, appID, secret,merchantID, Option<HttpContent>.None));

    public Task<RestApiResponse<T>> PostAsync<T>(string path)
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, string.Empty, Option<HttpContent>.None));

    public Task<RestApiResponse> PostAsync(string path, HttpContent content)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, string.Empty, content));

    public Task<RestApiResponse<T>> PostAsync<T>(string path, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, string.Empty, content));

    public Task<RestApiResponse<T>> PostAsync<T>(string path, HttpContent content, Dictionary<string, string> headers,
        MediaTypeWithQualityHeaderValue acceptHeader)
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, string.Empty, content, null, headers, acceptHeader));

    public Task<RestApiResponse> PostAsync(string path, string accessToken, HttpContent content)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, accessToken, content));

    public Task<RestApiResponse> PostAsync(string path, Guid appID, string secret, Guid merchantID, HttpContent content)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, appID, secret, merchantID, content));

    public Task<RestApiResponse> PostAsync(string path, string accessToken)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, accessToken, Option<HttpContent>.None));

    public Task<RestApiResponse> PostAsync(string path, Guid appID, string secret, Guid merchantID)
        => ExecAsync(BuildRequest(HttpMethod.Post, path, appID, secret, merchantID, Option<HttpContent>.None));

    public Task<RestApiResponse<T>> PostAsync<T>(string path, string accessToken, HttpContent content) 
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, accessToken, content));

    public Task<RestApiResponse<T>> PostAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Post, path, appID, secret, merchantID, content));

    public Task<RestApiResponse> PutAsync(string path)
        => ExecAsync(BuildRequest(HttpMethod.Put, path, string.Empty, Option<HttpContent>.None));

    public Task<RestApiResponse> PutAsync(string path, string accessToken)
        => ExecAsync(BuildRequest(HttpMethod.Put, path, accessToken, Option<HttpContent>.None));

    public Task<RestApiResponse> PutAsync(string path, Guid appID, string secret, Guid merchantID)
        => ExecAsync(BuildRequest(HttpMethod.Put, path, appID, secret, merchantID, Option<HttpContent>.None));

    public Task<RestApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Put, path, accessToken, content));

    public Task<RestApiResponse<T>> PutAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content)
        => ExecAsync<T>(BuildRequest(HttpMethod.Put, path, appID, secret, merchantID, content));

    public Task<RestApiResponse<T>> PutAsync<T>(string path, string accessToken, HttpContent content, string rowVersion)
        => ExecAsync<T>(BuildRequest(HttpMethod.Put, path, accessToken, content, rowVersion));

    public Task<RestApiResponse<T>> PutAsync<T>(string path, Guid appID, string secret, Guid merchantID, HttpContent content, string rowVersion)
        => ExecAsync<T>(BuildRequest(HttpMethod.Put, path, appID, secret, merchantID, content, rowVersion));

    public Task<RestApiResponse> DeleteAsync(string path)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, string.Empty, Option<HttpContent>.None));

    public Task<RestApiResponse> DeleteAsync(string path, string accessToken)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, accessToken, Option<HttpContent>.None));

    public Task<RestApiResponse> DeleteAsync(string path, Guid appID, string secret, Guid merchantID)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, appID, secret, merchantID, Option<HttpContent>.None));

    public Task<RestApiResponse> DeleteAsync(string path, string accessToken, string rowVersion)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, accessToken, Option<HttpContent>.None, rowVersion));

    public Task<RestApiResponse> DeleteAsync(string path, Guid appID, string secret, Guid merchantID, string rowVersion)
        => ExecAsync(BuildRequest(HttpMethod.Delete, path, appID, secret, merchantID, Option<HttpContent>.None, rowVersion));

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

    /// <summary>
    /// Builds a request that will use a HMAC signature for authetnication.
    /// </summary>
    /// <param name="method">The HTTP method for teh request.</param>
    /// <param name="path">The URL the request will be sent to.</param>
    /// <param name="appID">The application ID for the HMAC signature.</param>
    /// <param name="secret">The secret for the HMAC signature.</param>
    /// <param name="merchantID">The merchant ID for the HMAC signature.</param>
    /// <param name="httpContent">Optional HTTP content to include with the request.</param>
    /// <param name="rowVersion">Optional row version header to include with the request.</param>
    /// <param name="headers">Optional list of HTTP headers to set on the request.</param>
    /// <param name="acceptHeader">Optional HTTP accept header to set on the request.</param>
    /// <returns>A HTTP request message object ready to send to an HTTP server.</returns>
    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        Guid appID,
        string secret,
        Guid merchantID,
        Option<HttpContent> httpContent,
        string? rowVersion = null,
        Dictionary<string, string>? headers = null,
        MediaTypeWithQualityHeaderValue? acceptHeader = null)
    {
        var signatureHeaders = HmacSignatureBuilder.GetAppHeaders(
            appID.ToString(), 
            Guid.NewGuid().ToString(), 
            secret, 
            DateTime.UtcNow, 
            merchantID);

        if(headers == null)
        {
            headers = signatureHeaders;
        }
        else
        {
            foreach (var header in signatureHeaders)
            {
                headers.Add(header.Key, header.Value);
            }
        }

        return BuildRequest(method, path, string.Empty, httpContent, rowVersion, headers, acceptHeader);
    }

    private HttpRequestMessage BuildRequest(
        HttpMethod method,
        string path,
        string accessToken,
        Option<HttpContent> httpContent,
        string? rowVersion = null,
        Dictionary<string, string>? headers = null,
        MediaTypeWithQualityHeaderValue? acceptHeader = null)
    {
        HttpRequestMessage request = new HttpRequestMessage(method, path);

        request.Headers.Accept.Add(acceptHeader ?? new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        
        if (!string.IsNullOrEmpty(rowVersion))
        {
            request.Headers.TryAddWithoutValidation("If-Match", rowVersion);
        }
        
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        if(httpContent.IsSome)
        {
            request.Content = (HttpContent)httpContent;
        }

        return request;
    }

    private async Task<RestApiResponse> ToApiResponse(HttpResponseMessage response, Uri? requestUri)
    {
        if (response.IsSuccessStatusCode)
        {
            return new RestApiResponse(response.StatusCode, requestUri, response.Headers);
        }
        else if (response.Content.Headers.ContentLength > 0)
        {
            string contentStr = await response.Content.ReadAsStringAsync();
            var problem = DeserialiseProblem(response.StatusCode, contentStr);
            return new RestApiResponse(response.StatusCode, requestUri, response.Headers, problem);
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            return new RestApiResponse(response.StatusCode, requestUri, response.Headers, new NoFrixionProblem(response.StatusCode, error));
        }
    }

    private async Task<RestApiResponse<T>> ToApiResponse<T>(HttpResponseMessage response, Uri? requestUri)
    {
        if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength > 0)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var result = jsonString.FromJson<T>();

            return result != null ?
                new RestApiResponse<T>(response.StatusCode, requestUri, response.Headers, result) :
                new RestApiResponse<T>(response.StatusCode, requestUri, response.Headers,
                    new NoFrixionProblem(response.StatusCode, $"Json deserialisation failed for type {typeof(T)}."));
        }
        else if (response.Content.Headers.ContentLength > 0)
        {
            var contentStr = await response.Content.ReadAsStringAsync();
            var problem = DeserialiseProblem(response.StatusCode, contentStr);

            if (_dataExpectedOnErrorResponse)
            {
                var result = TryDeserialiseDataOnErrorResponse<T>(contentStr);
                
                if (result != null)
                {
                    return new RestApiResponse<T>(response.StatusCode, requestUri, response.Headers, problem, result);
                }
            }
            return new RestApiResponse<T>(response.StatusCode, requestUri, response.Headers, problem);
        }
        else
        {
            return new RestApiResponse<T>(response.StatusCode, requestUri, response.Headers, 
                new NoFrixionProblem(response.StatusCode, "Response content was expected."));
        }
    }
    
    private T? TryDeserialiseDataOnErrorResponse<T>(string contentStr)
    {
        try
        {
            return contentStr.FromJson<T>();
        }
        catch (Newtonsoft.Json.JsonException)
        {
            return default;
        }
    }

    /// <summary>
    /// Attempts to deserialise a problem object from the contents of a failure response.
    /// </summary>
    /// <param name="responseStatusCode">The failure HTTP status code of the response.</param>
    /// <param name="responseContent">The payload of the failure response.</param>
    /// <returns>A NoFrixionProblem instance.</returns>
    private NoFrixionProblem DeserialiseProblem(HttpStatusCode responseStatusCode, string responseContent)
    {
        try
        {
            return responseContent.FromJson<NoFrixionProblem>() ?? NoFrixionProblem.Empty;
        }
        catch (System.Text.Json.JsonException)
        {
            var problem = new NoFrixionProblem(responseStatusCode, $"API error response was not in the recognised problem format.");
            problem.RawError = responseContent;

            return problem;
        }
        catch (Newtonsoft.Json.JsonException)
        {
            var problem = new NoFrixionProblem(responseStatusCode, $"API error response was not in the recognised problem format.");
            problem.RawError = responseContent;

            return problem;
        }
    }

    private async Task<RestApiResponse> ExecAsync(
        HttpRequestMessage req,
        CancellationToken cancellationToken = default)
    {
        var response = await HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
        return await ToApiResponse(response, req.RequestUri);
    }

    private async Task<RestApiResponse<T>> ExecAsync<T>(
        HttpRequestMessage req,
        CancellationToken cancellationToken = default)
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
