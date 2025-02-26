// -----------------------------------------------------------------------------
//  Filename: AccountIdentifierCreate.cs
// 
//  Description: Account identifier Create model
//  Author(s):
//  saurav@nofrixion.com (saurav@nofrixion.com)
// 
//  History:
//  16 04 2024  Saurav Maiti   Created, Harcourt street, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class AccountIdentifierCreate
{
    /// <summary>
    /// The currency for the account.
    /// </summary>
    public CurrencyTypeEnum? Currency { get; set; }

    /// <summary>
    /// The Bank Identifier Code for an IBAN.
    /// </summary>
    private string _bic;
    public string BIC
    {
        get => _bic;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bic = value.Trim().Replace(" ", string.Empty);
            }
            else
            {
                _bic = value;
            }
        }
    }

    /// <summary>
    /// The International Bank Account Number for the identifier. Only applicable 
    /// for IBAN identifiers.
    /// </summary>
    private string _iban;
    public string IBAN 
    {
        get => _iban;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _iban = value.Trim().Replace(" ", string.Empty);
            }
            else
            {
                _iban = value;
            }
        }
    }

    /// <summary>
    /// The account Sort Code. Only applicable for SCAN identifiers.
    /// </summary>
    private string _sortCode;
    public string SortCode
    {
        get => _sortCode;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _sortCode = value.Trim().Replace(" ", string.Empty).Replace("-", string.Empty);
            }
            else
            {
                _sortCode = value;
            }
        }
    }

    /// <summary>
    /// Bank account number. Only applicable for SCAN identifiers.
    /// </summary>
    private string _accountNumber;
    public string AccountNumber
    {
        get => _accountNumber;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _accountNumber = value.Trim().Replace(" ", string.Empty);
            }
            else
            {
                _accountNumber = value;
            }
        }
    }

    /// <summary>
    /// Bitcoin address destination.
    /// </summary>
    private string _bitcoinAddress;

    [Obsolete]
    public string BitcoinAddress
    {
        get => _bitcoinAddress;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bitcoinAddress = value.Trim();
            }
            else
            {
                _bitcoinAddress = value;
            }
        }
    }

    public virtual Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        return new Dictionary<string, string>
        {
            { keyPrefix + nameof(Currency), Currency.ToString()},
            { keyPrefix + nameof(BIC), BIC ?? string.Empty},
            { keyPrefix + nameof(IBAN), IBAN ?? string.Empty},
            { keyPrefix + nameof(SortCode), SortCode ?? string.Empty},
            { keyPrefix + nameof(AccountNumber), AccountNumber ?? string.Empty}
        };
    }

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string Summary =>
        $"IBAN: {IBAN}, BIC: {BIC}, SortCode: {SortCode}, AccountNumber: {AccountNumber}";
}