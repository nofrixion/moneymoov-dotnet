// -----------------------------------------------------------------------------
//  Filename: CounterpartyMapper.cs
// 
//  Description: Mapping extensions for Counterparty model
//  Author(s):
//  saurav@nofrixion.com (saurav@nofrixion.com)
// 
//  History:
//  16 04 2024  Saurav Maiti   Created, Harcourt street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov;

public static class CounterpartyMapper
{
    public static Counterparty ToCounterparty(this CounterpartyCreate counterpartyCreate, CurrencyTypeEnum currency)
    {
        return new Counterparty
        {
            AccountID = counterpartyCreate.AccountID,
            Name = counterpartyCreate.Name,
            EmailAddress = counterpartyCreate.EmailAddress,
            PhoneNumber = counterpartyCreate.PhoneNumber,
            Identifier = counterpartyCreate.Identifier?.ToAccountIdentifier(currency)
        };
    }
    
    public static CounterpartyCreate ToCounterpartyCreate(this Counterparty counterparty)
    {
        return new CounterpartyCreate
        {
            AccountID = counterparty.AccountID,
            Name = counterparty.Name,
            EmailAddress = counterparty.EmailAddress,
            PhoneNumber = counterparty.PhoneNumber,
            Identifier = counterparty.Identifier?.ToAccountIdentifierCreate()
        };
    }
}