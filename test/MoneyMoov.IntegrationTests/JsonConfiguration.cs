//-----------------------------------------------------------------------------
// Filename: JsonConfiguration.cs
//
// Description: A JSON based configuration helper for integrations tests.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 27 Nov 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System.Reflection;

namespace NoFrixion.MoneyMoov.IntegrationTests;

public static class JsonConfiguration
{
    public static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)!.FullName)
            .AddJsonFile("appsettings.json", true)
            .AddEnvironmentVariables()
            .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
            .Build();
    }

    /// <summary>
    /// Builds a custom configuration object based on a key value pair dictionary.
    /// </summary>
    public static IConfiguration BuildCustom(Dictionary<string, string> settings)
    {
        MemoryConfigurationSource memConfigSrc = new MemoryConfigurationSource();
        memConfigSrc.InitialData =  settings.AsEnumerable();
        MemoryConfigurationProvider memProvider = new MemoryConfigurationProvider(memConfigSrc);
        return new ConfigurationRoot(new List<IConfigurationProvider> { memProvider });
    }
}
