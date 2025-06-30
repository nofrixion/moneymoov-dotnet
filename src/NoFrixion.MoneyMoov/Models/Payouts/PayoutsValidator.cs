// -----------------------------------------------------------------------------
//  Filename: PaymentsValidator.cs
// 
//  Description: A static class containing common payment validations.
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  22 02 2022  Donal O'Connor   Created, Carmichael House,
// Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using LanguageExt;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace NoFrixion.MoneyMoov.Models;

public static class PayoutsValidator
{
    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the Their Reference field
    /// for Modulr payments.
    /// </summary>
    [Obsolete("Modulr is no longer used.")]
    public const int THEIR_REFERENCE_MINIMUM_MODULR_LENGTH = 1;

    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the Their Reference field
    /// for Modulr GBP payments.
    /// </summary>
    [Obsolete("Modulr is no longer used. Use REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH instead.")]
    public const int THEIR_REFERENCE_SCAN_MAXIMUM_MODULR_LENGTH = REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH;

    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the Their Reference field
    /// for Modulr EUR payments.
    /// </summary>
    [Obsolete("Modulr is no longer used. Use REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH instead.")]
    public const int THEIR_REFERENCE_IBAN_MAXIMUM_MODULR_LENGTH = REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH;

    /// <summary>
    /// Maximum length of the Your, or External Reference, field.
    /// </summary>
    public const int YOUR_REFERENCE_MAXIMUM_LENGTH = 256;

    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the account name
    /// for Modulr.
    /// </summary>
    [Obsolete("Modulr is no longer used. Use BANKING_CIRCLE_ALLOWED_CHARS_REGEX instead.")]
    public const string MODULR_ACCOUNT_NAME_REGEX = BANKING_CIRCLE_ALLOWED_CHARS_REGEX;

    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the Their Reference field
    /// for Modulr payments.
    /// </summary>
    [Obsolete("Modulr is no longer used. Use BANKING_CIRCLE_ALLOWED_CHARS_REGEX instead.")]
    public const string THEIR_REFERENCE_MODULR_REGEX = BANKING_CIRCLE_ALLOWED_CHARS_REGEX;

    /// <summary>
    /// This constant is obsolete and is no longer used. It was used to validate the Their Reference field
    /// for Modulr payments.
    /// </summary>
    [Obsolete("Modulr is no longer used.")]
    public const string THEIR_REFERENCE_NON_COUNTED_CHARS_MODULR_REGEX = @"[\.\-/&\s]";


    /// <summary>
    /// The External Reference, or Your reference, field is the one that gets set locally on the 
    /// payer's transaction record. It does not get sent out through the payment network.
    /// The upstream supplier currently only support alphanumeric, space, hyphen(-) and underscore (_) characters.
    /// An empty value is also supported.
    /// </summary>
    [Obsolete("Your reference regex is no longer used. Use BANKING_CIRCLE_ALLOWED_CHARS_REGEX instead.")]
    public const string YOUR_REFERENCE_REGEX = @"^[\w\-\s]*$";

    /// <summary>
    /// Validation regex for the destination IBAN field.
    /// </summary>
    public const string IBAN_REGEX = @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]){11,30}$";

    /// <summary>
    /// Fallback pattern for TheirReference field.
    /// </summary>
    public const string FALLBACK_THEIR_REFERENCE = "NFXN {0}";

    /// <summary>
    /// Validation reqex for the Name amd Reference (Your and Their) fields with Banking Cirlce. It must 
    /// have at least one non space character.  Total of all characters must be 140 or less.
    /// Banking Circle supported chars see https://docs.bankingcircleconnect.com/docs/initiate-payments:
    /// a b c d e f g h i j k l m n o p q r s t u v w x y z
    /// A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
    /// 0 1 2 3 4 5 6 7 8 9
    /// / - ? : ( ) . , ' +
    /// In addition the field cannot start with a : or - character and must have one none space char.
    /// Space
    /// </summary>
    public const string BANKING_CIRCLE_ALLOWED_CHARS_REGEX = @"^(?![\:\-])[a-zA-Z0-9\s\/\-\.\+\(\)\?\:\,']*$";

    /// <summary>
    /// The maximum allowed length for the Banking Circle remittance information. Does not seem to differ
    /// for GBP (SCAN) and EUR (IBAN) payments.
    /// </summary>
    public const int REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH = 140;

    /// <summary>
    /// The maximum allowed length for the Banking Circle creditor account name.
    /// </summary>
    public const int ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH = 35;

    /// <summary>
    /// Number of days in the future that a payout can be scheduled for.
    /// </summary>
    public const int PAYOUT_SCHEDULE_DAYS_IN_FUTURE = 60;

    /// <summary>
    /// Validation error message template for Banking Cirlce string fields.
    /// </summary>
    public const string BANKING_CIRCLE_STRING_VALIDATION_ERROR_TEMPLATE = "The {0} field must consist of at least 1 non-blank character. " +
        "The allowed characters are alphanumeric, forward slash (/), hyphen (-), question mark (?), colon (:), parentheses, full stop (.), " +
        "comma (,), single quote ('), plus (+) and space. The total length must not exceed {1} characters. The first character cannot be ':' or '-'.";

    /// <summary>
    /// Fiat currencies are only allowed to be specified to two decimal places.
    /// </summary>
    public const decimal FIAT_CURRENCY_RESOLUTION = 0.01M;

    /// <summary>
    /// The minimum amount that must be set for the source or destination amount when dong a multi-currency payout.
    /// </summary>
    public const decimal FX_MINIMUM_QUOTE_AMOUNT = 1.00M;

    /// <summary>
    /// The required length for a SCAN account number string.
    /// </summary>
    private const int GBP_SCAN_REQUIRED_ACCOUNT_NUMBER_LENGTH = 8;
    
    /// <summary>
    /// The required length for a GBP SCAN sort code.
    /// </summary>
    private const int GBP_SCAN_REQUIRED_SORT_CODE_LENGTH = 6;

    /// <summary>
    /// The required length for a USD FedWire sort code.
    /// </summary>
    /// <remarks>
    /// See financialInstitution filed (need to horizontally scroll) on
    /// https://docs.bankingcircleconnect.com/docs/initiate-correspondent-and-agency-banking-payment
    /// </remarks>
    private const int USD_SCAN_REQUIRED_SORT_CODE_LENGTH = 9;

    public static bool ValidateIBAN(string iban)
    {
        if (string.IsNullOrEmpty(iban))
        {
            return false;
        }

        var bankAccount = iban.ToUpper().Trim().Replace(" ", string.Empty);

        if (!Regex.IsMatch(bankAccount, IBAN_REGEX))
        {
            return false;
        }

        var ibanLengths = new Dictionary<string, int> { { "IE", 22 } };

        // Extract country code and validate length
        if (ibanLengths.TryGetValue(bankAccount[..2], out int expectedLength))
        {
            if (bankAccount.Length != expectedLength)
            {
                return false;
            }
        }

        string bank = bankAccount[4..] + bankAccount[..4];

        int asciiShift = 55;

        var sb = new StringBuilder();

        foreach (char c in bank)
        {
            int v;
            if (char.IsLetter(c)) v = c - asciiShift;
            else v = int.Parse(c.ToString());
            sb.Append(v);
        }

        string checkSumString = sb.ToString();
        int checksum = int.Parse(checkSumString[..1]);

        for (int i = 1; i < checkSumString.Length; i++)
        {
            int v = int.Parse(checkSumString.Substring(i, 1));
            checksum *= 10;
            checksum += v;
            checksum %= 97;
        }

        return checksum == 1;
    }

    public static bool IsValidAccountName(
        string accountName, 
        PaymentProcessorsEnum processor)
    {
        if (string.IsNullOrEmpty(accountName))
        {
            return false;
        }

        return processor switch
        {
            PaymentProcessorsEnum.BankingCircle => ValidateBankingCircleStringField(accountName, ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH),
            PaymentProcessorsEnum.BankingCircleAgency => ValidateBankingCircleStringField(accountName, ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH),
            // If the payment processor is not known, or has no specific validations, perform all the validations.
            _ =>ValidateBankingCircleStringField(accountName, ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH)
        };
    }

    public static bool ValidateTheirReference(
        string? theirReference,
        AccountIdentifierType desinationIdentifierType,
        PaymentProcessorsEnum processor)
    {
        if (string.IsNullOrEmpty(theirReference))
        {
            return false;
        }

        return processor switch
        {
            PaymentProcessorsEnum.BankingCircle => ValidateBankingCircleStringField(theirReference, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH),
            PaymentProcessorsEnum.BankingCircleAgency => ValidateBankingCircleStringField(theirReference, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH),
            // If the payment processor is not known, or has no specific validations, perform all the validations.
            _ => ValidateBankingCircleStringField(theirReference, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH),
        };
    }
    
    [Obsolete("Modulr is no longer used. Banking Circle validation will be used instead.")]
    public static bool ValidateModulrAccountName(string accountName)
    {
        return ValidateBankingCircleStringField(accountName, ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH);
    }

    [Obsolete("Modulr is no longer used. Banking Circle validation will be used instead.")]
    public static bool ValidateModulrTheirReference(
        string theirReference,
        AccountIdentifierType desinationIdentifierType)
    {
        return ValidateBankingCircleStringField(theirReference, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH);
    }


    public static bool ValidateBankingCircleStringField(string field, int maxAllowedLength)
    {
        Regex matchRegex = new Regex(BANKING_CIRCLE_ALLOWED_CHARS_REGEX);

        if (string.IsNullOrEmpty(field?.Trim())
                || field.Length > maxAllowedLength
                || !matchRegex.IsMatch(field))
        {
            return false;
        }

        return true;
    }

    public static bool ValidateYourReference(string? yourReference)
    {
        if (string.IsNullOrEmpty(yourReference))
        {
            return true;
        }

        return ValidateBankingCircleStringField(yourReference, YOUR_REFERENCE_MAXIMUM_LENGTH);
    }

    public static IEnumerable<ValidationResult> Validate(Payout payout, ValidationContext validationContext)
    {
        if (payout == null)
        {
            yield break;
        }

        if (payout.Amount <= decimal.Zero && payout.FxDestinationAmount <= 0)
        {
            yield return new ValidationResult($"Either the payout {nameof(payout.Amount)} or {nameof(payout.FxDestinationAmount)} must be supplied.", [ nameof(payout.Amount), nameof(payout.FxDestinationAmount)]);
        }

        if(payout.FxUseDestinationAmount == true && (payout.FxDestinationAmount == null || payout.FxDestinationAmount <= 0))
        {
            yield return new ValidationResult($"FxDestinationAmount must be supplied when FxUseDestinationAmount is true.", [ nameof(payout.FxDestinationAmount) ]);
        }

        if (payout.FxUseDestinationAmount == true && payout.FxDestinationCurrency == null)
        {
            yield return new ValidationResult($"FxDestinationCurrency must be supplied when FxUseDestinationAmount is true.", [ nameof(payout.FxDestinationCurrency) ]);
        }

        if (payout.FxUseDestinationAmount == false && payout.Amount <= 0)
        {
            yield return new ValidationResult($"Amount must be supplied when FxUseDestinationAmount is false.", [ nameof(payout.Amount) ]);
        }

        if (payout.Amount > 0 && payout.Currency.IsFiat() && payout.Amount % FIAT_CURRENCY_RESOLUTION != 0)
        {
            yield return new ValidationResult($"The payout amount must only be specified to two decimal places for currency {payout.Currency}.", [ nameof(payout.Amount) ]);
        }

        if (payout.FxDestinationAmount > 0 && payout.FxDestinationCurrency != null && payout.FxDestinationCurrency.Value.IsFiat() && payout.FxDestinationAmount % FIAT_CURRENCY_RESOLUTION != 0)
        {
            yield return new ValidationResult($"The payout FxDestinationAmount must only be specified to two decimal places for currency {payout.FxDestinationCurrency}.", [nameof(payout.FxDestinationAmount)]);
        }

        if (payout.Amount < FX_MINIMUM_QUOTE_AMOUNT && payout.FxUseDestinationAmount == false && payout.FxDestinationCurrency != null)
        {
            yield return new ValidationResult($"The payout amount for a multi-currency payout must be at least {FX_MINIMUM_QUOTE_AMOUNT}.", [nameof(payout.Amount)]);
        }

        if (payout.FxUseDestinationAmount == true && payout.FxDestinationAmount < FX_MINIMUM_QUOTE_AMOUNT)
        {
            yield return new ValidationResult($"The payout FX destination amount for a multi-currency payout must be at least {FX_MINIMUM_QUOTE_AMOUNT}.", [nameof(payout.FxDestinationAmount)]);
        }

        if (payout.Destination == null)
        {
            yield return new ValidationResult("Destination account required.", [ nameof(payout.Destination) ]);
        }

        if(payout.Currency == payout.FxDestinationCurrency)
        {
            yield return new ValidationResult("FxDestinationCurrency must be different from Currency.", [ nameof(payout.FxDestinationCurrency) ]);
        }

        var destinationCurrency = payout.FxDestinationCurrency ?? payout.Currency;

        if (payout.Destination?.AccountID == null && payout.Destination?.BeneficiaryID == null)
        {
            // Only validate the identifier details if the account or beneficiary ID is not set.

            if (payout.Destination != null && payout.Destination?.Identifier == null)
            {
                yield return new ValidationResult("Destination account identifier required.", [ nameof(payout.Destination.Identifier) ]);
            }

            if (string.IsNullOrEmpty(payout.Destination?.Name))
            {
                yield return new ValidationResult("Destination account name required.", [ nameof(payout.Destination.Name) ]);
            }

            if (payout.Destination?.Name != null &&
                !IsValidAccountName(payout.Destination?.Name ?? string.Empty, payout.PaymentProcessor))
            {
                yield return new ValidationResult(
                    string.Format(BANKING_CIRCLE_STRING_VALIDATION_ERROR_TEMPLATE, "Destination Account Name",
                        ACCOUNT_NAME_MAXIMUM_BANKING_CIRCLE_LENGTH),
                    [nameof(payout.Destination.Name)]);
            }

            if (payout.Destination != null &&
                !(payout.Type == AccountIdentifierType.IBAN || payout.Type == AccountIdentifierType.SCAN || payout.Type == AccountIdentifierType.BIC))
            {
                yield return new ValidationResult("Only destination types of IBAN and SCAN are supported.", [ nameof(payout.Type) ]);
            }

            if(!string.IsNullOrWhiteSpace(payout.Destination?.Identifier?.IBAN) &&
                (!string.IsNullOrWhiteSpace(payout.Destination?.Identifier?.SortCode) || !string.IsNullOrWhiteSpace(payout.Destination?.Identifier?.AccountNumber)))
            {
                yield return new ValidationResult("Only one of the IBAN or SCAN details can be set, not both.", [nameof(payout.Destination.Identifier.IBAN), nameof(payout.Destination.Identifier.SortCode), nameof(payout.Destination.Identifier.AccountNumber)]);
            }

            if (payout.Type == AccountIdentifierType.IBAN && payout.Destination?.Identifier != null &&
                string.IsNullOrWhiteSpace(payout.Destination?.Identifier?.IBAN ?? string.Empty))
            {
                yield return new ValidationResult("The destination account IBAN must be specified for an IBAN payout type.",[ nameof(payout.Destination.Identifier.IBAN) ]);
            }

            if (payout.Type == AccountIdentifierType.IBAN && payout.Destination?.Identifier != null &&
                !ValidateIBAN(payout.Destination?.Identifier?.IBAN ?? string.Empty))
            {
                yield return new ValidationResult("Destination IBAN is invalid, Please enter a valid IBAN.", [ nameof(payout.Destination.Identifier.IBAN) ]);
            }

            // IBAN's can be used for all Fiat currencies
            if (payout.Type == AccountIdentifierType.IBAN && destinationCurrency.IsFiat() == false)
            {
                string destinationCurrencyField = payout.FxDestinationCurrency != null ? nameof(payout.FxDestinationCurrency) : nameof(payout.Currency);
                yield return new ValidationResult(
                    $"Currency {destinationCurrency} cannot be used with IBAN destinations.", [destinationCurrencyField]);
            }

            if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
                string.IsNullOrEmpty(payout.Destination?.Identifier?.SortCode))
            {
                yield return new ValidationResult("Destination sort code required for a SCAN payout type.", [ nameof(payout.Destination.Identifier.SortCode) ]);
            }

            if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
                string.IsNullOrEmpty(payout.Destination?.Identifier?.AccountNumber))
            {
                yield return new ValidationResult("Destination account number is required for a SCAN payout type.", [ nameof(payout.Destination.Identifier.AccountNumber) ]);
            }

            if (payout.Type == AccountIdentifierType.SCAN && destinationCurrency == CurrencyTypeEnum.EUR)
            {
                string destinationCurrencyField = payout.FxDestinationCurrency != null ? nameof(payout.FxDestinationCurrency) : nameof(payout.Currency);
                yield return new ValidationResult($"Currency {destinationCurrency} cannot be used with SCAN destinations.", [ destinationCurrencyField ]);
            }
            
            if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
                destinationCurrency == CurrencyTypeEnum.GBP &&
                !string.IsNullOrEmpty(payout.Destination?.Identifier?.AccountNumber) && payout.Destination.Identifier.AccountNumber.Length != GBP_SCAN_REQUIRED_ACCOUNT_NUMBER_LENGTH)
            {
                yield return new ValidationResult("Destination account number must be eight digits for a GBP SCAN payout type.", [ nameof(payout.Destination.Identifier.AccountNumber) ]);
            }
            
            if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
                destinationCurrency == CurrencyTypeEnum.GBP && 
                !string.IsNullOrEmpty(payout.Destination?.Identifier?.SortCode) && payout.Destination.Identifier.SortCode.Length != GBP_SCAN_REQUIRED_SORT_CODE_LENGTH)
            {
                yield return new ValidationResult("Destination sort code must be six digits for a GBP SCAN payout type.",[ nameof(payout.Destination.Identifier.SortCode) ]);
            }

            if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
                destinationCurrency == CurrencyTypeEnum.USD &&
                !string.IsNullOrEmpty(payout.Destination?.Identifier?.SortCode) && payout.Destination.Identifier.SortCode.Length != USD_SCAN_REQUIRED_SORT_CODE_LENGTH)
            {
                yield return new ValidationResult("Destination sort code must be nine digits for a USD SCAN payout type.", [nameof(payout.Destination.Identifier.SortCode)]);
            }

            if (payout.Destination?.Identifier != null)
            {
                foreach (var err in payout.Destination.Identifier.Validate(validationContext))
                {
                    yield return err;
                }
            }
        }

        if (!ValidateTheirReference(payout.TheirReference, payout.Type, payout.PaymentProcessor))
        {
            yield return new ValidationResult(
                string.Format(BANKING_CIRCLE_STRING_VALIDATION_ERROR_TEMPLATE, "Their Reference",
                    REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH),
                [nameof(payout.TheirReference)]);
        }

        if (!ValidateYourReference(payout.YourReference))
        {
            yield return new ValidationResult("Your reference can only contain alphanumeric, space, hyphen(-) and underscore (_) characters. " +
            $"The maximum allowed length of the field is {YOUR_REFERENCE_MAXIMUM_LENGTH} characters.",
            [ nameof(payout.YourReference) ]);
        }

        if (payout is { Scheduled: true, ScheduleDate: null })
        {
            yield return new ValidationResult($"ScheduleDate must have a value if Scheduled is true.", new string[] { nameof(payout.ScheduleDate) });
        }

        if (payout.Scheduled.GetValueOrDefault() && payout.ScheduleDate <= DateTimeOffset.UtcNow)
        {
            yield return new ValidationResult($"ScheduleDate {payout.ScheduleDate} must be in the future.", new string[] { nameof(payout.ScheduleDate) });
        }

        if (payout.Scheduled.GetValueOrDefault() && payout.ScheduleDate > DateTimeOffset.UtcNow.AddDays(PAYOUT_SCHEDULE_DAYS_IN_FUTURE))
        {
            yield return new ValidationResult($"ScheduleDate {payout.ScheduleDate} cannot be more than {PAYOUT_SCHEDULE_DAYS_IN_FUTURE} days in the future.", new string[] { nameof(payout.ScheduleDate) });
        }
    }

    /// <summary>
    /// Constructs a valid TheirReference field based on the desired string. The aim is to make it as close as
    /// desired to the supplied value while still remaining valid.
    /// </summary>
    /// <param name="identifierType">The account type the Their Reference field is for.</param>
    /// <param name="theirReference">The desired TheirReference string.</param>
    /// <param name="paymentProcessor">The payment processor the payout is being submitted for.</param>
    /// <returns>A safe TheirReference value.</returns>
    public static string MakeSafeTheirReference(AccountIdentifierType identifierType, string theirReference, PaymentProcessorsEnum paymentProcessor)
    {
        string fallbackTheirReference = string.Format(FALLBACK_THEIR_REFERENCE, DateTime.Now.ToString("HHmmss"));

        if (string.IsNullOrEmpty(theirReference))
        {
            return fallbackTheirReference;
        }

        if (ValidateTheirReference(theirReference, identifierType, paymentProcessor))
        {
            return theirReference.Trim();
        }
        else
        {
            theirReference = Regex.Replace(theirReference, @"[^a-zA-Z0-9 ]", "");

            theirReference = theirReference.Length > REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH
                ? theirReference.Substring(0, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH)
                : theirReference;

            return ValidateTheirReference(theirReference, identifierType, paymentProcessor)
                ? theirReference.Trim()
                : fallbackTheirReference;
        }
    }

    public static string GetYourReferenceFromInvoices(List<string?> invoiceReferences)
    {
        return GetDelimitedStringInRange(invoiceReferences, YOUR_REFERENCE_MAXIMUM_LENGTH);
    }

    public static string GetTheirReferenceFromInvoices(CurrencyTypeEnum currency, List<string?> invoiceReferences, PaymentProcessorsEnum paymentProcessor)
    {
        return MakeSafeTheirReference(
            currency == CurrencyTypeEnum.EUR ? AccountIdentifierType.IBAN : AccountIdentifierType.SCAN, 
            GetDelimitedStringInRange(invoiceReferences, REFERENCE_MAXIMUM_BANKING_CIRCLE_LENGTH),
            paymentProcessor);
    }

    /// <summary>
    /// Builds a delimted string from a list of strings, ensuring the total length does not exceed the character limit.
    /// </summary>
    /// <param name="collection">The collection of strings</param>
    /// <param name="characterLimit">The character limit</param>
    /// <returns>A delimited string</returns>
    private static string GetDelimitedStringInRange(List<string?> collection, int characterLimit)
    {
        const string more = " ...";

        var sb = new StringBuilder(characterLimit);
        var remaining = collection.Count;

        foreach (var s in collection)
        {
            if (string.IsNullOrEmpty(s))
            {
                continue;
            }

            if (remaining == collection.Count && s.Length > characterLimit)
            {
                return s[..(characterLimit - 3)] + "...";
            }

            if (sb.Length + 2 + s.Length > characterLimit - more.Length)
            {
                sb.Append(more);
                break;
            }

            if (sb.Length > 0)
            {
                sb.Append(" . ");
            }

            sb.Append(s);
            remaining--;
        }

        return sb.ToString();
    }
}