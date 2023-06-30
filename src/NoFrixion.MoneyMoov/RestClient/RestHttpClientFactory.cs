//-----------------------------------------------------------------------------
// Filename: RestHttpClientFactory.cs
//
// Description: Factory class for creating REST clients.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 18 Jun 2023  Aaron Clauson   Refactored from RestApiClient.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public sealed class RestHttpClientFactory : IHttpClientFactory, IDisposable
{
    private readonly HttpClient _httpClient;
    private bool _disposed;

    public RestHttpClientFactory(Uri baseUri)
    {
        var handler = new HttpClientHandler();
        _httpClient = new HttpClient(handler, disposeHandler: true);
        _httpClient.BaseAddress = baseUri;
    }

    public HttpClient CreateClient(string name)
    {
        if (_disposed) throw new ObjectDisposedException(nameof(RestHttpClientFactory));
        return _httpClient;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _httpClient.Dispose();
            _disposed = true;
        }
    }
}
