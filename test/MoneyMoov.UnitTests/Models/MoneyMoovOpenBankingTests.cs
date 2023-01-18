//-----------------------------------------------------------------------------
// Filename: MoneyMoovOpenBankingTests.cs
//
// Description: Unit tests for the MoneyMoov OpenBanking models.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 22 Dec 2022  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models.OpenBanking;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class MoneyMoovOpenBankingTests : MoneyMoovUnitTestBase<MoneyMoovOpenBankingTests>
{
    public MoneyMoovOpenBankingTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that an empty open banking account model can be serialised.
    /// </summary>
    [Fact]
    public void Serialise_Empty_Account_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var account = new Account();

        Logger.LogDebug(account.ToJson());
        Logger.LogDebug(account.ToJson());
    }

    /// <summary>
    /// Tests that an empty open banking transaction model can be serialised.
    /// </summary>
    [Fact]
    public void Serialise_Empty_Transaction_Test()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var tx = new Transaction();

        Logger.LogDebug(tx.ToJson());
        Logger.LogDebug(tx.ToJson());
    }
}