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
    public static bool ValidateIBAN(string iban)
    {
        var bankAccount = iban.ToUpper();

        if (string.IsNullOrEmpty(bankAccount))
        {
            return false;
        }

        if (!Regex.IsMatch(bankAccount, "^[A-Z0-9]"))
        {
            return false;
        }

        bankAccount = bankAccount.Replace(" ", String.Empty);
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

    public static bool IsValidAccount(Regex accountRegex, string accountName)
    {
        return !string.IsNullOrEmpty(accountName) && accountRegex.IsMatch(accountName);
    }

    public static bool ValidateTheirReference(string theirReference)
    {
        Regex matchRegex = new Regex(@"^[A-Za-z0-9-\s\.\/& ]{6,}");

        Regex replaceRegex = new Regex(@"[\.\-/& ]");
        var refClean = replaceRegex.Replace(theirReference, "");

        //check at least 6 char is alphanumeric with total char less than 18
        if (   refClean.Length < 6 
            || theirReference.Length > 18 
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