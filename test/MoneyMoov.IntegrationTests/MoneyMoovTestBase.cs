//-----------------------------------------------------------------------------
// Filename: MoneyMoovTestBase.cs
// 
// Description: Base class for MoneyMoov integration tests.
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

using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.IntegrationTests;

public class MoneyMoovTestBase<T> where T : class
{
    private sealed class DefaultHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new HttpClient();
    }

    protected readonly string SandboxAccessToken;

    public MoneyMoovTestBase(ITestOutputHelper testOutputHelper)
    {
        LoggerFactory = new LoggerFactory();
        LoggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        Logger = LoggerFactory.CreateLogger<T>();

        Configuration = JsonConfiguration.BuildConfiguration();

        HttpClientFactory = new DefaultHttpClientFactory();

        SandboxAccessToken = Configuration["MoneyMoov:SandboxAccessToken"];
    }

    public LoggerFactory LoggerFactory { get; set; }

    public ILogger Logger { get; set; }

    public IConfiguration Configuration { get; set; }

    public IHttpClientFactory HttpClientFactory { get; set; }
}
