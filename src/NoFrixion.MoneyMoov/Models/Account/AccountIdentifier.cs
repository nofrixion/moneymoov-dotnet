// -----------------------------------------------------------------------------
//  Filename: AccountIdentifier.cs
// 
//  Description: Account identifier:
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//  19 09 2023  Aaron Clauson    Added Bitcoin support.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class AccountIdentifier
{
    /// <summary>
    /// The type of the account identifier.
    /// </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public AccountIdentifierType Type
    {
        get
        {
            if (!string.IsNullOrEmpty(IBAN))
            {
                return AccountIdentifierType.IBAN;
            }

            if (!string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(AccountNumber))
            {
                return AccountIdentifierType.SCAN;
            }

            if (!string.IsNullOrEmpty(BitcoinAddress))
            {
                return AccountIdentifierType.Bitcoin;
            }

            // Return default
            return AccountIdentifierType.Unknown;
        }
    }

    /// <summary>
    /// The currency for the account.
    /// </summary>
    public string Currency { get; set; }

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
                _sortCode = value.Trim().Replace(" ", string.Empty);
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
    public string BitcoinAddress
    {
        get => _accountNumber;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _bitcoinAddress = value.Trim().Replace(" ", string.Empty);
            }
            else
            {
                _bitcoinAddress = value;
            }
        }
    }

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string Summary =>   
        Type == AccountIdentifierType.IBAN ? Type.ToString() + ": " + IBAN :
        Type == AccountIdentifierType.SCAN ? Type.ToString() + ": " + SortCode + " / " + AccountNumber :
         "No identifier.";

    public virtual Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        return new Dictionary<string, string>
        {
            { keyPrefix + nameof(Currency), Currency ?? string.Empty},
            { keyPrefix + nameof(BIC), BIC ?? string.Empty},
            { keyPrefix + nameof(IBAN), IBAN ?? string.Empty},
            { keyPrefix + nameof(SortCode), SortCode ?? string.Empty},
            { keyPrefix + nameof(AccountNumber), AccountNumber ?? string.Empty},
            { keyPrefix + nameof(BitcoinAddress), BitcoinAddress ?? string.Empty}
        };
    }

    public string GetApprovalHash()
    {
        string input =
            (!string.IsNullOrEmpty(Currency) ? Currency.ToString() : string.Empty) +
            (!string.IsNullOrEmpty(BIC) ? BIC : string.Empty) +
            (!string.IsNullOrEmpty(IBAN) ? IBAN : string.Empty) +
            (!string.IsNullOrEmpty(SortCode) ? SortCode : string.Empty) +
            (!string.IsNullOrEmpty(AccountNumber) ? AccountNumber : string.Empty) +
            (!string.IsNullOrEmpty(BitcoinAddress) ? BitcoinAddress : string.Empty);
        return HashHelper.CreateHash(input);
    }

    public override string ToString()
    {
        return $"Type: {Type}, Currency: {Currency}, BIC: {BIC}, IBAN: {IBAN}, SortCode: {SortCode}, AccountNumber: {AccountNumber}, Bitcoin Address: {BitcoinAddress}, Summary: {Summary}";
    }
}