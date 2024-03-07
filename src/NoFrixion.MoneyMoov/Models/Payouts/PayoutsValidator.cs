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
    /// The minimum required length for the Their Reference field. Note that length gets 
    /// calculated after certain non-counted characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_MINIMUM_LENGTH = 6;

    /// <summary>
    /// The maximum allowed length for the Their Reference field for sort code
    /// and account number (SCAN) payments . Note that length gets calculated after 
    /// certain non-counted characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_SCAN_MAXIMUM_LENGTH = 17;

    /// <summary>
    /// The maximum allowed length for the Their Reference field for International Bank Account Number
    /// (IBAN) payments . Note that length gets calculated after / certain non-counted characters 
    /// have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_IBAN_MAXIMUM_LENGTH = 139;

    /// <summary>
    /// Maximum length of the Your, or External Reference, field.
    /// </summary>
    public const int YOUR_REFERENCE_MAXIMUM_LENGTH = 50;

    /// <summary>
    /// Validation regex for the destination account name field.
    /// </summary>
    /// <remarks>
    /// The original regular expression, from the supplier's swagger file was adjusted to match
    /// what the original intent seems to have been. The original expression allowed any string
    /// as long as it had at least one letter or number.
    /// The intent seems to have been to allow only letters (unicode), numbers and '.-/&amp; and spaces
    /// and it must contain at least one letter or number.
    /// "Beneficiary name can only have alphanumerics plus full stop, hyphen, forward slash or ampersand"
    /// </remarks>
    //public const string ACCOUNT_NAME_REGEX = @"^([^\p{L}0-9]*?[\p{L}0-9]){1,}['\.\-\/&\s]*";
    public const string ACCOUNT_NAME_REGEX = @"^['\.\-\/&\s]*?\w+['\.\-\/&\s\w]*$";

    /// <summary>
    /// Validation reqex for the Their, or Reference, field. It  must consist of at least 6 
    /// alphanumeric characters that are not all the same. Optional, uncounted characters include space, 
    /// hyphen(-), full stop (.), ampersand(&amp;), and forward slash (/). Total of all characters must be less than 
    /// 18 for scan payment and 140 for an IBAN payment.
    /// Somewhat misleadingly, the Reference field cannot contain a hyphen, the allowed characters are:
    /// alpha numeric (including unicode), space, hyphen(-), full stop (.), ampersand(&amp;), and forward slash (/). 
    /// </summary>
    /// <remarks>
    /// [^\W_] is actings as \w with the underscore character included. The upstream supplier does not permit
    /// underscore in the Reference (Theirs) field.
    /// </remarks>
    public const string THEIR_REFERENCE_REGEX = @"^([^\W_]|[\.\-/&\s]){6,}$";

    /// <summary>
    /// Certain characters in the Their Reference field are not counted towards the minimum and
    /// maximum length requirements. This regex indicates the list of allow characters that are NOT
    /// counted.
    /// </summary>
    public const string THEIR_REFERENCE_NON_COUNTED_CHARS_REGEX = @"[\.\-/&\s]";

    /// <summary>
    /// The External Reference, or Your reference, field is the one that gets set locally on the 
    /// payer's transaction record. It does not get sent out through the payment network.
    /// The upstream supplier currently only support alphanumeric, space, hyphen(-) and underscore (_) characters.
    /// An empty value is also supported.
    /// </summary>
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
    /// Number of days in the future that a payout can be scheduled for.
    /// </summary>
    public const int PAYOUT_SCHEDULE_DAYS_IN_FUTURE = 60;
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

    public static bool ValidateYourReference(string? yourReference)
    {
        if (string.IsNullOrEmpty(yourReference))
        {
            return true;
        }

        Regex matchRegex = new Regex(YOUR_REFERENCE_REGEX);

        if (yourReference.Length > YOUR_REFERENCE_MAXIMUM_LENGTH
            || !matchRegex.IsMatch(yourReference))
        {
            return false;
        }

        return true;
    }

    public static IEnumerable<ValidationResult> Validate(Payout payout, ValidationContext validationContext)
    {
        if (payout == null)
        {
            yield break;
        }

        if (payout.Amount <= decimal.Zero)
        {
            yield return new ValidationResult("The payment amount must be more than 0.", new string[] { nameof(payout.Amount) });
        }

        if (payout.Destination == null)
        {
            yield return new ValidationResult("Destination account required.", new string[] { nameof(payout.Destination) });
        }

        if (payout.Destination != null && payout.Destination?.Identifier == null)
        {
            yield return new ValidationResult("Destination account identifier required.", new string[] { nameof(payout.Destination.Identifier) });
        }

        if (string.IsNullOrEmpty(payout.Destination?.Name))
        {
            yield return new ValidationResult("Destination account name required.", new string[] { nameof(payout.Destination.Name) });
        }

        if (payout.Destination?.Name != null && !IsValidAccountName(payout.Destination?.Name ?? string.Empty))
        {
            yield return new ValidationResult($"Destination account name is invalid. It can only contain alphanumeric characters plus the ' . - & and space characters.",
                new string[] { nameof(payout.Destination.Name) });
        }

        if (payout.Destination != null && 
            !(payout.Type == AccountIdentifierType.IBAN || payout.Type == AccountIdentifierType.SCAN || payout.Type == AccountIdentifierType.BTC))
        {
            yield return new ValidationResult("Only destination types of IBAN, SCAN or BTC are supported.", new string[] { nameof(payout.Type) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && payout.Destination?.Identifier != null &&
            string.IsNullOrEmpty(payout.Destination?.Identifier?.IBAN ?? string.Empty))
        {
            yield return new ValidationResult("The destination account IBAN must be specified for an IBAN payout type.", new string[] { nameof(payout.Destination.Identifier.IBAN) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && payout.Destination?.Identifier != null &&
            !ValidateIBAN(payout.Destination?.Identifier?.IBAN ?? string.Empty))
        {
            yield return new ValidationResult("Destination IBAN is invalid, Please enter a valid IBAN.", new string[] { nameof(payout.Destination.Identifier.IBAN) });
        }

        if (payout.Type == AccountIdentifierType.IBAN && payout.Currency != CurrencyTypeEnum.EUR)
        {
            yield return new ValidationResult($"Currency {payout.Currency} cannot be used with IBAN destinations.", new string[] { nameof(payout.Currency) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
            string.IsNullOrEmpty(payout.Destination?.Identifier?.SortCode))
        {
            yield return new ValidationResult("Destination sort code required for a SCAN payout type.", new string[] { nameof(payout.Destination.Identifier.SortCode) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && payout.Destination?.Identifier != null &&
            string.IsNullOrEmpty(payout.Destination?.Identifier?.AccountNumber))
        {
            yield return new ValidationResult("Destination account number is required for a SCAN payout type.", new string[] { nameof(payout.Destination.Identifier.AccountNumber) });
        }

        if (payout.Type == AccountIdentifierType.SCAN && payout.Currency != CurrencyTypeEnum.GBP)
        {
            yield return new ValidationResult($"Currency {payout.Currency} cannot be used with SCAN destinations.", new string[] { nameof(payout.Currency) });
        }

        if (payout.Type != AccountIdentifierType.BTC && !ValidateTheirReference(payout.TheirReference, payout.Type))
        {
            yield return new ValidationResult("Their reference must consist of at least 6 alphanumeric characters that are not all the same " +
                "(non alphanumeric characters do not get counted towards this minimum value). " +
                "The allowed characters are alphanumeric, space, hyphen(-), full stop (.), ampersand (&), and forward slash (/). " +
                "Total of all characters must be less than 18 for a SCAN payout and less than 140 for an IBAN payout.",
                new string[] { nameof(payout.TheirReference) });
        }

        if (!ValidateYourReference(payout.YourReference))
        {
            yield return new ValidationResult("Your reference can only contain alphanumeric, space, hyphen(-) and underscore (_) characters. " +
            $"The maximum allowed length of the field is {YOUR_REFERENCE_MAXIMUM_LENGTH} characters.",
            new string[] { nameof(payout.YourReference) });
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
    /// <returns>A safe TheirReference value.</returns>
    public static string MakeSafeTheirReference(AccountIdentifierType identifierType, string theirReference)
    {
        string fallbackTheirReference = string.Format(FALLBACK_THEIR_REFERENCE, DateTime.Now.ToString("HHmmss"));

        if (string.IsNullOrEmpty(theirReference))
        {
            return fallbackTheirReference;
        }

        if (ValidateTheirReference(theirReference, identifierType))
        {
            return theirReference;
        }
        else
        {
            theirReference = Regex.Replace(theirReference, @"[^a-zA-Z0-9 ]", "");

            int theirRefMaxLength = identifierType == AccountIdentifierType.SCAN ?
               THEIR_REFERENCE_SCAN_MAXIMUM_LENGTH :
               THEIR_REFERENCE_IBAN_MAXIMUM_LENGTH;

            theirReference = theirReference.Length > theirRefMaxLength ?
                theirReference.Substring(0, theirRefMaxLength) :
                theirReference;

            theirReference = theirReference.Length < THEIR_REFERENCE_MINIMUM_LENGTH ?
                theirReference.PadRight(THEIR_REFERENCE_MINIMUM_LENGTH - theirReference.Length, 'X') :
                theirReference;

            return ValidateTheirReference(theirReference, identifierType) ? theirReference : fallbackTheirReference;
        }
    }
    
    public static string GetYourReferenceFromInvoices(List<string?> invoiceReferences)
    {
        return GetDelimitedStringInRange(invoiceReferences, YOUR_REFERENCE_MAXIMUM_LENGTH);
    }
    
    public static string GetTheirReferenceFromInvoices(CurrencyTypeEnum currency, List<string?> invoiceReferences)
    {
        var maxLength = currency == CurrencyTypeEnum.GBP ? THEIR_REFERENCE_SCAN_MAXIMUM_LENGTH : THEIR_REFERENCE_IBAN_MAXIMUM_LENGTH;
        
        return MakeSafeTheirReference(currency == CurrencyTypeEnum.EUR ? AccountIdentifierType.IBAN : AccountIdentifierType.SCAN, GetDelimitedStringInRange(invoiceReferences, maxLength));
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
        
        foreach(var s in collection)
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