//-----------------------------------------------------------------------------
// Filename: TransactionSerialisationTests.cs
//
// Description: Unit tests for the serialisation and deserialisation of the
// MoneyMoov Transaction models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 18 Mar 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class TransactionSerialisationTests : MoneyMoovUnitTestBase<TransactionSerialisationTests>
{
    public TransactionSerialisationTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that a transaction can be serialised successfully.
    /// </summary>
    [Fact]
    public void Serialise_Transaction_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var tx = new Transaction();

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        options.Converters.Add(new JsonStringEnumConverter());
        var txJson = System.Text.Json.JsonSerializer.Serialize<Transaction>(tx, options);

        Assert.NotEmpty(txJson);
    }
}