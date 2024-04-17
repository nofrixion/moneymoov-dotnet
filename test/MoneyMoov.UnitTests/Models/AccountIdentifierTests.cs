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
            IBAN = " GB42MO CK00000070629 907 ",
            Currency = CurrencyTypeEnum.EUR
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
            AccountNumber = " 00000070629 907 ",
            Currency = CurrencyTypeEnum.EUR
        };

        Assert.Equal(AccountIdentifierType.SCAN, accountIdentifier.Type);
        Assert.Equal("123456", accountIdentifier.SortCode);
        Assert.Equal("00000070629907", accountIdentifier.AccountNumber);
    }

    [Fact]
    public void AccountIdentifier_Validate_EUR_Success()
    {
        var accountIdentifier = new AccountIdentifier
        {
            BIC = "MODR123",
            IBAN = "GB42MOCK00000070629907",
            Currency = CurrencyTypeEnum.EUR
        };
        
        var validationResults = accountIdentifier.Validate();
        
        Assert.True(validationResults.IsEmpty);
    }
    
    [Fact]
    public void AccountIdentifier_Validate_GBP_Success()
    {
        var accountIdentifier = new AccountIdentifier
        {
            SortCode = "123456",
            AccountNumber = "00000070629907",
            Currency = CurrencyTypeEnum.GBP
        };
        
        var validationResults = accountIdentifier.Validate();
        
        Assert.True(validationResults.IsEmpty);
    }
    
    [Fact]
    public void AccountIdentifier_Validate_EUR_No_IBAN_Failure()
    {
        var accountIdentifier = new AccountIdentifier
        {
            BIC = "MODR123",
            Currency = CurrencyTypeEnum.EUR
        };
        
        var validationResults = accountIdentifier.Validate();
        
        Assert.False(validationResults.IsEmpty);
    }
    
    [Fact]
    public void AccountIdentifier_Validate_GBP_No_SortCode_Failure()
    {
        var accountIdentifier = new AccountIdentifier
        {
            AccountNumber = "00000070629907",
            Currency = CurrencyTypeEnum.GBP
        };
        
        var validationResults = accountIdentifier.Validate();
        
        Assert.False(validationResults.IsEmpty);
    }
    
    [Fact]
    public void AccountIdentifier_Validate_GBP_No_AccountNumber_Failure()
    {
        var accountIdentifier = new AccountIdentifier
        {
            SortCode = "123456",
            Currency = CurrencyTypeEnum.GBP
        };
        
        var validationResults = accountIdentifier.Validate();
        
        Assert.False(validationResults.IsEmpty);
    }
    
}
