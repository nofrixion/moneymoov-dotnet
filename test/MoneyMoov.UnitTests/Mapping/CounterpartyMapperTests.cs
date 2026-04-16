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

    /// <summary>
    /// Tests that ToCounterparty maps address fields from CounterpartyCreate.
    /// </summary>
    [Fact]
    public void MapToCounterparty_FromCounterpartyCreate_MapsAddressFields()
    {
        var counterpartyCreate = new CounterpartyCreate
        {
            AccountID = Guid.NewGuid(),
            Name = "Test Counterparty",
            AddressLine1 = "123 Main Street",
            AddressLine2 = "Suite 456",
            PostCode = "EC1A 1BB",
            PostTown = "London"
        };

        var counterparty = counterpartyCreate.ToCounterparty(CurrencyTypeEnum.EUR);

        Assert.Equal("123 Main Street", counterparty.AddressLine1);
        Assert.Equal("Suite 456", counterparty.AddressLine2);
        Assert.Equal("EC1A 1BB", counterparty.PostCode);
        Assert.Equal("London", counterparty.PostTown);
    }

    /// <summary>
    /// Tests that ToCounterpartyCreate maps address fields from Counterparty.
    /// </summary>
    [Fact]
    public void MapToCounterpartyCreate_FromCounterparty_MapsAddressFields()
    {
        var counterparty = new Counterparty
        {
            Name = "Test Counterparty",
            AddressLine1 = "456 High Road",
            AddressLine2 = "Floor 2",
            PostCode = "W1A 0AX",
            PostTown = "Manchester"
        };

        var counterpartyCreate = counterparty.ToCounterpartyCreate();

        Assert.Equal("456 High Road", counterpartyCreate.AddressLine1);
        Assert.Equal("Floor 2", counterpartyCreate.AddressLine2);
        Assert.Equal("W1A 0AX", counterpartyCreate.PostCode);
        Assert.Equal("Manchester", counterpartyCreate.PostTown);
    }

    /// <summary>
    /// Tests that address fields are null when not set on the source CounterpartyCreate.
    /// </summary>
    [Fact]
    public void MapToCounterparty_FromCounterpartyCreate_NullAddressFields()
    {
        var counterpartyCreate = new CounterpartyCreate
        {
            AccountID = Guid.NewGuid(),
            Name = "Test Counterparty"
        };

        var counterparty = counterpartyCreate.ToCounterparty(CurrencyTypeEnum.EUR);

        Assert.Null(counterparty.AddressLine1);
        Assert.Null(counterparty.AddressLine2);
        Assert.Null(counterparty.PostCode);
        Assert.Null(counterparty.PostTown);
    }
}