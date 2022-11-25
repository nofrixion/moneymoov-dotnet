// -----------------------------------------------------------------------------
//  Filename: AccountIdentifier.cs
// 
//  Description: Account identifier:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class AccountIdentifier
{
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    public AccountIdentifierType? Type { get; set; }

    /// <summary>
    /// Bank account Sort Code
    /// </summary>
    /// <value>Bank account Sort Code</value>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or Sets Bic
    /// </summary>
    public string Bic { get; set; }

    /// <summary>
    /// Gets or Sets CountrySpecificDetails
    /// </summary>
    public CountrySpecificDetails CountrySpecificDetails { get; set; }

    /// <summary>
    /// Gets or Sets Currency
    /// </summary>
    public string Currency { get; set; }

    /// <summary>
    /// Gets or Sets Iban
    /// </summary>
    public string Iban { get; set; }

    /// <summary>
    /// Bank account Sort Code
    /// </summary>
    /// <value>Bank account Sort Code</value>
    public string SortCode { get; set; }

    public override string ToString()
    {
        return $"{nameof(Type)}: {Type}, {nameof(AccountNumber)}: {AccountNumber}, {nameof(Bic)}: {Bic}, {nameof(CountrySpecificDetails)}: {CountrySpecificDetails}, {nameof(Currency)}: {Currency}, {nameof(Iban)}: {Iban}, {nameof(SortCode)}: {SortCode}";
    }
}