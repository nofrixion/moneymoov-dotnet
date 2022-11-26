using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace NoFrixion.MoneyMoov.IntegrationTests;

public static class JsonConfiguration
{
    public static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddJsonFile("appsettings.json", true)
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
