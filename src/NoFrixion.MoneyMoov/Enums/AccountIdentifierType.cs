// -----------------------------------------------------------------------------
//  Filename: AccountIdentifierType.cs
// 
//  Description: PaymentAccount identifier type:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

/// <summary>
/// Defines the different types of account identifiers that are supported.
/// </summary>
public enum AccountIdentifierType
{
    /// <summary>
    /// For cases when the account identifier is not recognised.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Sort code and account number. Used by the UK faster payments network.
    /// </summary>
    SCAN = 1,

    /// <summary>
    /// International bank account number. Used by the Single European Payment Area (SEPA) networks.
    /// </summary>
    IBAN = 2,

    /// <summary>
    /// Direct debit.
    /// </summary>
    DD = 3,

    /// <summary>
    /// Bitcoin address.
    /// </summary>
    BTC = 4,

    /// <summary>
    /// Bank Identifier Code. Used for international payments. In addition to the BIC an account number and country code are required.
    /// </summary>
    BIC = 5
}