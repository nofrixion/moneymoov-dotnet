using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace NoFrixion.MoneyMoov.UnitTests;

public class AccountIdentifierMapperTests
{
    [Fact]
    public void MapToAccountIdentifier_FromAccountIdentifierCreate_Success()
    {
        var accountIdentifierCreate = new AccountIdentifierCreate()
        {
            IBAN = "GB33BUKB20201555555555",
            BIC = "BUKBGB22",
            AccountNumber = "20201555555555",
            SortCode = "202015",
            Currency = CurrencyTypeEnum.EUR
        };
        
        var accountIdentifier = accountIdentifierCreate.ToAccountIdentifier(CurrencyTypeEnum.GBP);
        
        Assert.Equal(accountIdentifierCreate.IBAN, accountIdentifier.IBAN);
        Assert.Equal(accountIdentifierCreate.BIC, accountIdentifier.BIC);
        Assert.Equal(accountIdentifierCreate.AccountNumber, accountIdentifier.AccountNumber);
        Assert.Equal(accountIdentifierCreate.SortCode, accountIdentifier.SortCode);
        Assert.Equal(accountIdentifierCreate.Currency, accountIdentifier.Currency);
    }
    
    [Fact]
    public void MapToAccountIdentifier_FromAccountIdentifierCreateWithoutCurrency_Success()
    {
        var accountIdentifierCreate = new AccountIdentifierCreate()
        {
            IBAN = "GB33BUKB20201555555555",
            BIC = "BUKBGB22",
            AccountNumber = "20201555555555",
            SortCode = "202015"
        };
        
        var accountIdentifier = accountIdentifierCreate.ToAccountIdentifier(CurrencyTypeEnum.EUR);
        
        Assert.Equal(accountIdentifierCreate.IBAN, accountIdentifier.IBAN);
        Assert.Equal(accountIdentifierCreate.BIC, accountIdentifier.BIC);
        Assert.Equal(accountIdentifierCreate.AccountNumber, accountIdentifier.AccountNumber);
        Assert.Equal(accountIdentifierCreate.SortCode, accountIdentifier.SortCode);
        Assert.Equal(CurrencyTypeEnum.EUR, accountIdentifier.Currency);
    }
    
    [Fact]
    public void MapToAccountIdentifierCreate_FromAccountIdentifier_Success()
    {
        var accountIdentifier = new AccountIdentifier()
        {
            IBAN = "GB33BUKB20201555555555",
            BIC = "BUKBGB22",
            AccountNumber = "20201555555555",
            SortCode = "202015",
            Currency = CurrencyTypeEnum.EUR
        };
        
        var accountIdentifierCreate = accountIdentifier.ToAccountIdentifierCreate();
        
        Assert.Equal(accountIdentifier.IBAN, accountIdentifierCreate.IBAN);
        Assert.Equal(accountIdentifier.BIC, accountIdentifierCreate.BIC);
        Assert.Equal(accountIdentifier.AccountNumber, accountIdentifierCreate.AccountNumber);
        Assert.Equal(accountIdentifier.SortCode, accountIdentifierCreate.SortCode);
        Assert.Equal(accountIdentifier.Currency, accountIdentifierCreate.Currency);
    }
}