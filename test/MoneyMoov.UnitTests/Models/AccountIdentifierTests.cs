//-----------------------------------------------------------------------------
// Filename: AccountIdentifierTests.cs
//
// Description: Unit tests for the MoneyMoov AccountIdentifier model.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Jun 2023  Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using NoFrixion.MoneyMoov.Models;
using Xunit;
using Xunit.Abstractions;

namespace NoFrixion.MoneyMoov.UnitTests;

public class AccountIdentifierTests : MoneyMoovUnitTestBase<AccountIdentifierTests>
{
    public AccountIdentifierTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
    { }

    /// <summary>
    /// Tests that spaces are removed from the BIC and IBAN when creating an account identifier.
    /// </summary>
    [Fact]
    public void AccountIdentifier_Create_IBAN_Spaces_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var accountIdentifier = new AccountIdentifier
        {
            BIC = "  MODR  123 ",
            IBAN = " GB42MO CK00000070629 907 "
        };

        Assert.Equal(AccountIdentifierType.IBAN, accountIdentifier.Type);
        Assert.Equal("MODR123", accountIdentifier.BIC);
        Assert.Equal("GB42MOCK00000070629907", accountIdentifier.IBAN);
    }

    /// <summary>
    /// Tests that spaces are removed from the Sort Code and Account Number when creating an account identifier.
    /// </summary>
    [Fact]
    public void AccountIdentifier_Create_SCAN_Spaces_Success()
    {
        Logger.LogDebug($"--> {TypeExtensions.GetCaller()}.");

        var accountIdentifier = new AccountIdentifier
        {
            SortCode = " 12 34 56 ",
            AccountNumber = " 00000070629 907 "
        };

        Assert.Equal(AccountIdentifierType.SCAN, accountIdentifier.Type);
        Assert.Equal("123456", accountIdentifier.SortCode);
        Assert.Equal("00000070629907", accountIdentifier.AccountNumber);
    }
}
