//-----------------------------------------------------------------------------
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
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NoFrixion.MoneyMoov
{
    public interface IMoneyMoovApiClient
    {
        Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path);

        Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path, string accessToken);

        Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, HttpContent content);

        Uri GetBaseUri();
    }

    public class MoneyMoovApiClient : IMoneyMoovApiClient
    {
        public const string HTTP_CLIENT_NAME = "moneymoov";

        private readonly HttpClient _httpClient;

        public MoneyMoovApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Uri GetBaseUri()
        {
            return _httpClient.BaseAddress ?? new Uri(MoneyMoovUrlBuilder.DEFAULT_MONEYMOOV_BASE_URL);
        }

        public Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path)
        {
            return ExecAsync<T>(BuildRequest(HttpMethod.Get, path, string.Empty));
        }

        public Task<MoneyMoovApiResponse<T>> GetAsync<T>(string path, string accessToken)
        {
            return ExecAsync<T>(BuildRequest(HttpMethod.Get, path, accessToken));
        }

        public Task<MoneyMoovApiResponse<T>> PostAsync<T>(string path, HttpContent content)
        {
            return ExecAsync<T>(BuildRequest(HttpMethod.Post, path, string.Empty, content));
        }

        private HttpRequestMessage BuildRequest(
           HttpMethod method,
           string path,
           string accessToken)
        {
            return BuildRequest(method, path, accessToken, Option<HttpContent>.None); 
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

        private async Task<MoneyMoovApiResponse<T>> ToApiResponse<T>(HttpResponseMessage response, Uri? requestUri)
        {
            if (response.IsSuccessStatusCode && response.Content.Headers.ContentLength > 0)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return result != null ?
                    new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, result) :
                    new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, 
                        NoFrixionProblemDetails.DeserialisationFailure(response.StatusCode, $"Json deserialisation failed for type {typeof(T)}."));
            }
            else if((int)response.StatusCode >= 400 && (int)response.StatusCode < 500 && response.Content.Headers.ContentLength > 0)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<NoFrixionProblemDetails>();
                return problemDetails != null ?
                    new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, problemDetails) :
                    new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers, 
                       NoFrixionProblemDetails.DeserialisationFailure(response.StatusCode, $"Json deserialisation failed for type {typeof(NoFrixionProblemDetails)}."));
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();

                return new MoneyMoovApiResponse<T>(response.StatusCode, requestUri, response.Headers,
                    new NoFrixionProblemDetails
                    {
                        Status = (int)response.StatusCode,
                        Title = response.ReasonPhrase,
                        Detail = error
                    });
            }
        }

        private async Task<MoneyMoovApiResponse<T>> ExecAsync<T>(
            HttpRequestMessage req,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = await _httpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);
            return await ToApiResponse<T>(response, req.RequestUri);
        }
    }
}
