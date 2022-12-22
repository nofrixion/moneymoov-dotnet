//-----------------------------------------------------------------------------
// Filename: MoneyMoovUnitTestBase.cs
// 
// Description: Base class for MoneyMoov unit tests.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 Nov 2022  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class MoneyMoovUnitTestBase<T> where T : class
{
    public MoneyMoovUnitTestBase(ITestOutputHelper testOutputHelper)
    {
        LoggerFactory = new LoggerFactory();
        LoggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
        Logger = LoggerFactory.CreateLogger<T>();
    }

    public LoggerFactory LoggerFactory { get; set; }

    public ILogger Logger { get; set; }
}
