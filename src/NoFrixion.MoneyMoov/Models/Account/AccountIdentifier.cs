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

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class AccountIdentifier: IValidatableObject
{
    public const int SORT_CODE_LENGTH = 6;

    /// <summary>
    /// The type of the account identifier.
    /// </summary>
    public AccountIdentifierType Type
    {
        get
        {
            if(Currency == CurrencyTypeEnum.GBP)
            {
                // UK Faster Payments can support both SCAN and IBAN identifiers. Default to SCAN.
                if (!string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(AccountNumber))
                {
                    return AccountIdentifierType.SCAN;
                }
                else if(!string.IsNullOrEmpty(IBAN))
                {
                    return AccountIdentifierType.IBAN;
                }
            }

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
                return AccountIdentifierType.BTC;
            }

            // Return default
            return AccountIdentifierType.Unknown;
        }
    }

    /// <summary>
    /// The currency for the account.
    /// </summary>
    public required CurrencyTypeEnum Currency { get; set; }

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

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string Summary =>   
        Type == AccountIdentifierType.IBAN ? Type.ToString() + ": " + IBAN :
        Type == AccountIdentifierType.SCAN ? Type.ToString() + ": " + DisplayScanSummary :
        Type == AccountIdentifierType.BTC ? Type.ToString() + ": " + BitcoinAddress :
         "No identifier.";
    
    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string DisplaySummary =>   
        Type == AccountIdentifierType.IBAN ? IBAN :
        Type == AccountIdentifierType.SCAN ? DisplayScanSummary :
        Type == AccountIdentifierType.BTC ? BitcoinAddress :
        "No identifier.";

    public string DisplayScanSummary =>
        !string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(AccountNumber) && SortCode.Length == SORT_CODE_LENGTH
            ? $"{SortCode[..2]}-{SortCode.Substring(2, 2)}-{SortCode.Substring(4, 2)} {AccountNumber}"
            : "No identifier.";

    public bool IsSameDestination(AccountIdentifier other)
    {
        if(other == null)
        {
            return false;
        }

        if (Type != other.Type)
        {
            return false;
        }

        return Type switch
        {
            AccountIdentifierType.IBAN => IBAN == other.IBAN,
            AccountIdentifierType.SCAN => SortCode == other.SortCode && AccountNumber == other.AccountNumber,
            AccountIdentifierType.BTC => BitcoinAddress == other.BitcoinAddress,
            _ => false
        };
    }

    public virtual Dictionary<string, string> ToDictionary(string keyPrefix)
    {
        return new Dictionary<string, string>
        {
            { keyPrefix + nameof(Currency), Currency.ToString()},
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
            Currency +
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

    public NoFrixionProblem Validate()
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(this, serviceProvider: null, items: null);
        var isValid = Validator.TryValidateObject(this, validationContext, validationResults, true);
        
        if (!isValid)
        {
            return new NoFrixionProblem($"The {nameof(AccountIdentifier)} had one or more validation errors.", validationResults);
        }
        
        return NoFrixionProblem.Empty;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        switch (Currency)
        {
            case CurrencyTypeEnum.GBP:
            {
                if (string.IsNullOrEmpty(SortCode) || string.IsNullOrEmpty(AccountNumber))
                {
                    yield return new ValidationResult(
                        "Sort code and account number are required for GBP account identifier.",
                        new[] { nameof(SortCode), nameof(AccountNumber) });
                }

                break;
            }
            case CurrencyTypeEnum.EUR:
            {
                if (string.IsNullOrEmpty(IBAN))
                {
                    yield return new ValidationResult("IBAN is required for EUR account identifier.",
                        new[] { nameof(IBAN) });
                }

                break;
            }
            case CurrencyTypeEnum.BTC:
            {
                if (string.IsNullOrEmpty(BitcoinAddress))
                {
                    yield return new ValidationResult("Bitcoin address is required for BTC account identifier.",
                        new[] { nameof(BitcoinAddress) });
                }

                break;
            }
            default:
            {
                yield return new ValidationResult("Currency is required for account identifier.",
                    new[] { nameof(Currency) });
                break;
            }
        }
    }
}