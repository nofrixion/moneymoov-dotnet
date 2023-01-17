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

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace NoFrixion.MoneyMoov.Models;

public static class PayoutsValidator
{
    /// <summary>
    /// The minimum required legnth for the Their Reference field. Note that that length gets 
    /// calcaulated after certain non-counter characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_MINIMUM_LENGTH = 6;

    /// <summary>
    /// The maximum allowed legnth for the Their Reference field for sort code
    /// and account number (SCAN) payments . Note that that length gets calculated after 
    /// certain non-counter characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_SCAN_MAXIMUM_LENGTH = 17;

    /// <summary>
    /// The maximum allowed legnth for the Their Reference field for International Bank Account Number
    /// (IBAN) payments . Note that that length gets calculated after / certain non-counter characters 
    /// have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_IBAN_MAXIMUM_LENGTH = 139;

    /// <summary>
    /// Validation regex for the destination account name field.
    /// </summary
    /// <remarks>
    /// The original regular expression, from the supplier's swagger file was adjusted to match
    /// what the original intent seems to have been. The original expression allowed any string
    /// as long as it had at least one letter or number.
    /// The intent seems to have been to allow only letters (unicode), numbers and '.-/& and spaces
    /// and it must contain at least one letter or number.
    /// "Beneficiary name can only have alphanumerics plus full stop, hyphen, forward slash or ampersand"
    /// </remarks>
    //public const string ACCOUNT_NAME_REGEX = @"^([^\p{L}0-9]*?[\p{L}0-9]){1,}['\.\-\/&\s]*";
    public const string ACCOUNT_NAME_REGEX = @"^['\.\-\/&\s]*?\w+['\.\-\/&\s\w]*$";

    /// <summary>
    /// Validation reqex for the Their, or external, Reference field. It  must consist of at least 6 
    /// alphanumeric characters that are not all the same. Optional, uncounted characters include space, 
    /// hyphen(-), full stop (.), ampersand(&), and forward slash (/). Total of all characters must be less than 
    /// 18 for scan payment and 140 for an IBAN payment.
    /// Somewhat contradictingly, validation error message states: External Reference can only have alphanumeric 
    /// characters plus underscore, hyphen and space.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public const string THEIR_REFERENCE_REGEX = @"^[\w\-_\s]{6,}$";

    /// <summary>
    /// Certain characters in the Their Reference field are not counterd towards the minimum and
    /// maximum length requirements. This regex indicates the list of allow characters that are NOT
    /// counted.
    /// </summary>
    public const string THEIR_REFERENCE_NON_COUNTED_CHARS_REGEX = @"[\.\-/&\s]";

    /// <summary>
    /// Validation regex for the destination IBAN field.
    /// </summary>
    public const string IBAN_REGEX = @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]){11,30}$";

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

    public static bool IsValidAccountName(string accountName)
    {
        var accountNameRegex = new Regex(ACCOUNT_NAME_REGEX);

        return !string.IsNullOrEmpty(accountName) && accountNameRegex.IsMatch(accountName);
    }

    public static bool ValidateTheirReference(string? theirReference, AccountIdentifierType desinationIdentifierType)
    {
        if (string.IsNullOrEmpty(theirReference))
        {
            return false;
        }

        int maxLength = desinationIdentifierType == AccountIdentifierType.IBAN ?
            THEIR_REFERENCE_IBAN_MAXIMUM_LENGTH : THEIR_REFERENCE_SCAN_MAXIMUM_LENGTH;

        Regex matchRegex = new Regex(THEIR_REFERENCE_REGEX);

        Regex replaceRegex = new Regex(THEIR_REFERENCE_NON_COUNTED_CHARS_REGEX);
        var refClean = replaceRegex.Replace(theirReference, "");

        if (refClean.Length < THEIR_REFERENCE_MINIMUM_LENGTH
            || refClean.Length > maxLength
            || !matchRegex.IsMatch(theirReference))
        {
            return false;
        }

        return theirReference.ToCharArray().Distinct().Count() > 1;
    }

    public static bool ValidateCurrency(string currency)
    {
        return Enum.TryParse<CurrencyTypeEnum>(currency, out var currencyParsed)
               && (currencyParsed == CurrencyTypeEnum.GBP
                   || currencyParsed == CurrencyTypeEnum.EUR);
    }

    public static bool ValidatePaymentRequestCurrency(CurrencyTypeEnum currency, PaymentMethodTypeEnum paymentMethodTypes)
    {
        return !(currency == CurrencyTypeEnum.LBTC &&
               (paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.card) ||
                paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp)));
    }

    public static bool ValidatePaymentRequestAmount(decimal? amount, PaymentMethodTypeEnum paymentMethodTypes, CurrencyTypeEnum currency)
    {
        return paymentMethodTypes switch
        {
            _ when paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp) && currency == CurrencyTypeEnum.EUR => amount >= PaymentsConstants.PISP_MINIMUM_EUR_PAYMENT_AMOUNT,
            _ when paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.pisp) && currency == CurrencyTypeEnum.GBP => amount >= PaymentsConstants.PISP_MINIMUM_GBP_PAYMENT_AMOUNT,
            _ when paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.cardtoken) => amount >= PaymentsConstants.CARD_MINIMUM_PAYMENT_AMOUNT,
            _ when paymentMethodTypes.HasFlag(PaymentMethodTypeEnum.card) => amount == decimal.Zero || amount >= PaymentsConstants.CARD_MINIMUM_PAYMENT_AMOUNT,
            _ => amount >= decimal.Zero
        };
    }

    public static IEnumerable<ValidationResult> Validate(IPayout payout, ValidationContext validationContext)
    {
        if (payout == null)
        {
            yield break;
        }

        if (!ValidateCurrency(payout.Currency!))
        {
            yield return new ValidationResult("The Payout currency was not recognised. Currently only EUR and GBP are supported.");
        }

        if (payout.Amount.GetValueOrDefault() <= decimal.Zero)
        {
            yield return new ValidationResult("The payment amount must be more than 0.");
        }

        if (string.IsNullOrEmpty(payout.DestinationAccount?.Name))
        {
            yield return new ValidationResult("Destination account name required");
        }
        else if (!IsValidAccountName(payout.DestinationAccount?.Name!))
        {
            yield return new ValidationResult($"Destination Account Name is invalid. It can only contain alaphanumberic characters plus the ' . - & and space characters.");
        }

        if (string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.IBAN) && string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.AccountNumber))
        {
            yield return new ValidationResult("Either the destination account IBAN or SCAN details need to be specified.");
        }

        if (!string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.IBAN) && !string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.AccountNumber))
        {
            yield return new ValidationResult("Only one of the destination account IBAN or SCAN details can be specified.");
        }

        if (payout.DestinationAccount?.Identifier?.Type == AccountIdentifierType.IBAN && !ValidateIBAN(payout.DestinationAccount?.Identifier?.IBAN!))
        {
            yield return new ValidationResult("Destination IBAN is invalid, Please enter a valid IBAN.");
        }

        if (!string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.IBAN) && payout.Currency != CurrencyTypeEnum.EUR.ToString())
        {
            yield return new ValidationResult($"Currency {payout.Currency} can only be used with IBAN destinations.");
        }

        if (payout.DestinationAccount?.Identifier?.Type == AccountIdentifierType.SCAN && string.IsNullOrEmpty(payout.DestinationAccount?.Identifier?.SortCode))
        {
            yield return new ValidationResult("Sort code required.");
        }

        if (payout.DestinationAccount?.Identifier?.Type == AccountIdentifierType.SCAN && !int.TryParse(payout.DestinationAccount?.Identifier?.AccountNumber, out _))
        {
            yield return new ValidationResult("Account number invalid. Please enter a valid account number.");
        }

        if (payout.DestinationAccount?.Identifier?.Type == AccountIdentifierType.SCAN && (payout.DestinationAccount?.Identifier?.SortCode.Length > 6 || !int.TryParse(payout.DestinationAccount?.Identifier?.SortCode, out _)))
        {
            yield return new ValidationResult("Sort code invalid. Please enter a valid sort code.");
        }

        if (payout.DestinationAccount?.Identifier?.Type == AccountIdentifierType.SCAN && payout.Currency != CurrencyTypeEnum.GBP.ToString())
        {
            yield return new ValidationResult($"Currency {payout.Currency} can only be used with SCAN destinations.");
        }

        if (!ValidateTheirReference(payout.TheirReference, payout!.DestinationAccount!.Identifier!.Type!))
        {
            yield return new ValidationResult("Their reference must consist of at least 6 alphanumeric characters that are not all the same. " +
                "Optional, uncounted characters include space, hyphen(-), full stop (.), ampersand(&), and forward slash (/). Total of all characters must be less than 18.");
        }
    }
}