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

using System.Text;
using System.Text.RegularExpressions;

namespace NoFrixion.MoneyMoov.Validators;

public static class PaymentsValidator
{
    /// <summary>
    /// The minimum required legnth for the Their Reference field. Note that that length gets 
    /// calcaulated after certain non-counter characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_MINIMUM_LENGTH = 6;

    /// <summary>
    /// The maximum allowed legnth for the Their Reference field. Note that that length gets 
    /// calcualted after certain non-counter characters have been removed.
    /// </summary>
    public const int THEIR_REFERENCE_MAXIMUM_LENGTH = 6;

    /// <summary>
    /// Validation regex for the destination account name field.
    /// </summary>
    public const string ACCOUNT_NAME_REGEX = @"^([^\p{L}0-9]*?[\p{L}0-9]){1,}['\.\-\/&\s]*";

    /// <summary>
    /// Validation reqex for the Their Reference field.
    /// </summary>
    public const string THEIR_REFERENCE_REGEX = @"^[A-Za-z0-9\-\.\/\& ]{6,}$";

    /// <summary>
    /// Certain characters in the Their Reference field are not counterd towards the minimum and
    /// maximum length requirements. This regex indicates the list of allow characters that are NOT
    /// counted.
    /// </summary>
    public const string THEIR_REFERENCE_NON_COUNTED_CHARS_REGEX = @"[\.\-/& ]";

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

        var bankAccount = iban.ToUpper().Trim().Replace(" ", String.Empty);

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

    public static bool ValidateTheirReference(string theirReference)
    {
        Regex matchRegex = new Regex(THEIR_REFERENCE_REGEX);

        Regex replaceRegex = new Regex(THEIR_REFERENCE_NON_COUNTED_CHARS_REGEX);
        var refClean = replaceRegex.Replace(theirReference, "");

        //check at least 6 char is alphanumeric with total char less than 18
        if (   refClean.Length < THEIR_REFERENCE_MINIMUM_LENGTH
            || theirReference.Length > THEIR_REFERENCE_MAXIMUM_LENGTH
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
}