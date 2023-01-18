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

        if (payout.Amount <= decimal.Zero)
        {
            yield return new ValidationResult("The payment amount must be more than 0.", new string[] { nameof(payout.Amount) });
        }

        if (string.IsNullOrEmpty(payout.DestinationAccountName))
        {
            yield return new ValidationResult("Destination account name required", new string[] { nameof(payout.DestinationAccountName) });
        }
        
        if (!IsValidAccountName(payout.DestinationAccountName))
        {
            yield return new ValidationResult($"Destination Account Name is invalid. It can only contain alaphanumberic characters plus the ' . - & and space characters.",
                new string[] { nameof(payout.DestinationAccountName) });
        }

        if (!(payout.Type == AccountIdentifierType.IBAN || payout.Type == AccountIdentifierType.SCAN))
        {
            yield return new ValidationResult("Only payouts of type IBAN or SCAN are supported.", new string[] { nameof(payout.Type) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && string.IsNullOrEmpty(payout.DestinationIBAN))
        {
            yield return new ValidationResult("The destination account IBAN must be specified for an IBAN payout type.", new string[] { nameof(payout.DestinationIBAN) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && !ValidateIBAN(payout.DestinationIBAN))
        {
            yield return new ValidationResult("Destination IBAN is invalid, Please enter a valid IBAN.", new string[] { nameof(payout.DestinationIBAN) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && payout.Currency != CurrencyTypeEnum.EUR)
        {
            yield return new ValidationResult($"Currency {payout.Currency} cannot be used with IBAN destinations.", new string[] { nameof(payout.Currency) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && string.IsNullOrEmpty(payout.DestinationSortCode))
        {
            yield return new ValidationResult("Sort code required for a SCAN payout type.", new string[] { nameof(payout.DestinationSortCode) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && string.IsNullOrEmpty(payout.DestinationAccountNumber))
        {
            yield return new ValidationResult("Account Number is required for a SCAN payout type.", new string[] { nameof(payout.DestinationAccountNumber) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && payout.Currency != CurrencyTypeEnum.GBP)
        {
            yield return new ValidationResult($"Currency {payout.Currency} cannot be used with SCAN destinations.", new string[] { nameof(payout.Currency) });
        }

        if (!ValidateTheirReference(payout.TheirReference, !string.IsNullOrEmpty(payout.DestinationAccountNumber) ? 
            AccountIdentifierType.SCAN : AccountIdentifierType.IBAN))
        {
            yield return new ValidationResult("Their reference must consist of at least 6 alphanumeric characters that are not all the same. " +
                "Optional, uncounted characters include space, hyphen(-) and underscore (_). Total of all characters must be less than 18 for a SCAN payout and less than 140 for an IBAN payout.",
                new string[] { nameof(payout.TheirReference) });
        }
    }
}