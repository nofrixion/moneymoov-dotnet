// -----------------------------------------------------------------------------
//  Filename: AccountIdentifierMapper.cs
// 
//  Description: Mapping extensions for AccountIdentifier model
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

public static class AccountIdentifierMapper
{
    public static AccountIdentifier ToAccountIdentifier(this AccountIdentifierCreate accountIdentifierCreate, CurrencyTypeEnum currency)
    {
        return new AccountIdentifier
        {
            AccountNumber = accountIdentifierCreate.AccountNumber,
            IBAN = accountIdentifierCreate.IBAN,
            BIC = accountIdentifierCreate.BIC,
            SortCode = accountIdentifierCreate.SortCode,
            Currency = accountIdentifierCreate.Currency ?? currency
        };
    }
    
    public static AccountIdentifierCreate ToAccountIdentifierCreate(this AccountIdentifier accountIdentifier)
    {
        return new AccountIdentifierCreate
        {
            AccountNumber = accountIdentifier.AccountNumber,
            IBAN = accountIdentifier.IBAN,
            BIC = accountIdentifier.BIC,
            SortCode = accountIdentifier.SortCode,
            Currency = accountIdentifier.Currency
        };
    }
}