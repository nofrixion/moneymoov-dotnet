using NoFrixion.MoneyMoov.Models;
using Xunit;

namespace NoFrixion.MoneyMoov;

public class CounterpartyMapperTests
{
    [Fact]
    public void MapToCounterparty_FromCounterpartyCreate_Success()
    {
        var counterpartyCreate = new CounterpartyCreate()
        {
            AccountID = Guid.NewGuid(),
            Name = "Test Counterparty",
            EmailAddress = "email@email.com",
            PhoneNumber = "1234567890",
            Identifier = new AccountIdentifierCreate()
            {
                    IBAN = "GB33BUKB20201555555555",
                    BIC = "BUKBGB22",
                    AccountNumber = "20201555555555",
                    SortCode = "202015",
                    Currency = CurrencyTypeEnum.EUR
            }
        };
        
        var counterparty = counterpartyCreate.ToCounterparty(CurrencyTypeEnum.GBP);
        
        Assert.Equal(counterpartyCreate.AccountID, counterparty.AccountID);
        Assert.Equal(counterpartyCreate.Name, counterparty.Name);
        Assert.Equal(counterpartyCreate.EmailAddress, counterparty.EmailAddress);
        Assert.Equal(counterpartyCreate.PhoneNumber, counterparty.PhoneNumber);
        Assert.Equal(counterpartyCreate.Identifier.IBAN, counterparty.Identifier?.IBAN);
        Assert.Equal(counterpartyCreate.Identifier.BIC, counterparty.Identifier?.BIC);
        Assert.Equal(counterpartyCreate.Identifier.AccountNumber, counterparty.Identifier?.AccountNumber);
        Assert.Equal(counterpartyCreate.Identifier.SortCode, counterparty.Identifier?.SortCode);
        Assert.Equal(counterpartyCreate.Identifier.Currency, counterparty.Identifier?.Currency);
    }

    [Fact]
    public void MapToCounterparty_FromCounterpartyCreateWithoutCurrency_Success()
    {
        var counterpartyCreate = new CounterpartyCreate()
        {
            AccountID = Guid.NewGuid(),
            Name = "Test Counterparty",
            EmailAddress = "email@email.com",
            PhoneNumber = "1234567890",
            Identifier = new AccountIdentifierCreate()
            {
                IBAN = "GB33BUKB20201555555555",
                BIC = "BUKBGB22",
                AccountNumber = "20201555555555",
                SortCode = "202015"
            }
        };

        var counterparty = counterpartyCreate.ToCounterparty(CurrencyTypeEnum.EUR);

        Assert.Equal(counterpartyCreate.AccountID, counterparty.AccountID);
        Assert.Equal(counterpartyCreate.Name, counterparty.Name);
        Assert.Equal(counterpartyCreate.EmailAddress, counterparty.EmailAddress);
        Assert.Equal(counterpartyCreate.PhoneNumber, counterparty.PhoneNumber);
        Assert.Equal(counterpartyCreate.Identifier.IBAN, counterparty.Identifier?.IBAN);
        Assert.Equal(counterpartyCreate.Identifier.BIC, counterparty.Identifier?.BIC);
        Assert.Equal(counterpartyCreate.Identifier.AccountNumber, counterparty.Identifier?.AccountNumber);
        Assert.Equal(counterpartyCreate.Identifier.SortCode, counterparty.Identifier?.SortCode);
        Assert.Equal(CurrencyTypeEnum.EUR, counterparty.Identifier?.Currency);
    }
}