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

public class AccountIdentifier : IValidatableObject
{
    public const int GBP_SORT_CODE_LENGTH = 6;

    /// <summary>
    /// The type of the account identifier.
    /// </summary>
    public AccountIdentifierType Type
    {
        get
        {
            if(!string.IsNullOrWhiteSpace(IBAN))
            {
                return AccountIdentifierType.IBAN;
            }
            else if(!string.IsNullOrWhiteSpace(SortCode) && !string.IsNullOrWhiteSpace(AccountNumber))
            {
                return AccountIdentifierType.SCAN;
            }

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

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string Summary =>
        Type == AccountIdentifierType.IBAN ? Type.ToString() + ": " + IBAN :
        Type == AccountIdentifierType.SCAN ? Type.ToString() + ": " + DisplayScanSummary :
         "No identifier.";

    /// <summary>
    /// Summary of the account identifier's most important properties.
    /// </summary>
    public string DisplaySummary =>
        Type == AccountIdentifierType.IBAN ? IBAN :
        Type == AccountIdentifierType.SCAN ? DisplayScanSummary :
        "No identifier.";

    public string DisplayScanSummary =>
        Currency == CurrencyTypeEnum.GBP && !string.IsNullOrEmpty(SortCode) && !string.IsNullOrEmpty(AccountNumber) && SortCode.Length == GBP_SORT_CODE_LENGTH
            ? $"{SortCode[..2]}-{SortCode.Substring(2, 2)}-{SortCode.Substring(4, 2)} {AccountNumber}"
            : $"{SortCode} {AccountNumber}";

    public bool IsSameDestination(AccountIdentifier other)
    {
        if (other == null)
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
            { keyPrefix + nameof(AccountNumber), AccountNumber ?? string.Empty}
        };
    }

    public string GetApprovalHash()
    {
        string input =
            Currency +
            (!string.IsNullOrEmpty(BIC) ? BIC : string.Empty) +
            (!string.IsNullOrEmpty(IBAN) ? IBAN : string.Empty) +
            (!string.IsNullOrEmpty(SortCode) ? SortCode : string.Empty) +
            (!string.IsNullOrEmpty(AccountNumber) ? AccountNumber : string.Empty);
        return HashHelper.CreateHash(input);
    }

    public override string ToString()
    {
        return $"Type: {Type}, Currency: {Currency}, BIC: {BIC}, IBAN: {IBAN}, SortCode: {SortCode}, AccountNumber: {AccountNumber}, Summary: {Summary}";
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
            // GBP & USD support both IBAN and SCAN.
            case CurrencyTypeEnum.GBP:
            case CurrencyTypeEnum.USD:
                {
                    if (string.IsNullOrWhiteSpace(IBAN) && 
                        (string.IsNullOrWhiteSpace(SortCode) || string.IsNullOrWhiteSpace(AccountNumber)))
                    {
                        yield return new ValidationResult(
                            $"Either the IBAN or Sort code and account number are required for a {Currency} account identifier.",
                            [nameof(IBAN), nameof(SortCode), nameof(AccountNumber)]);
                    }

                    break;
                }

            // EUR only supports IBAN.
            case CurrencyTypeEnum.EUR:
                {
                    if (string.IsNullOrEmpty(IBAN))
                    {
                        yield return new ValidationResult("IBAN is required for EUR account identifier.",
                             [nameof(IBAN)]);
                    }

                    break;
                }

            default:
                {
                    yield return new ValidationResult($"Currency {Currency} was not recognised when validating an account identifier.",
                        [nameof(Currency)]);
                    break;
                }
        }
    }
}