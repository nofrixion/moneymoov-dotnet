//-----------------------------------------------------------------------------
// Filename: PaymentAccountCreate.cs
// 
// Description: Model that can be supplied to the MoneyMoov API to create a mew
// payment account.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 11 Dec 2022  Aaron Clauson   Renamed from PaymentAccountCreate to PaymentAccountCreate.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentAccountCreate
{
    /// <summary>
    /// The ID of the merchant the payment account is being created for.
    /// </summary>
    public Guid MerchantID { get; set;}

    /// <summary>
    /// Currency for the account, only EUR and GBP are supported.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; } = CurrencyTypeEnum.EUR;

    /// <summary>
    /// The name for the account. This name is descriptive only, and will not be
    /// used when sending payments, but it should still be set to something that 
    /// identifies the purpose of the funds. For example "EUR Payment" would be
    /// suitable to for an account used for general purpose business payments.
    /// </summary>
    public string AccountName { get; set; } = string.Empty;

    /// <summary>
    /// Places all the payment request's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payment request's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        var dict = new Dictionary<string, string>();

        dict.Add(nameof(MerchantID), MerchantID.ToString());
        dict.Add(nameof(Currency), Currency.ToString());
        dict.Add(nameof(AccountName), AccountName ?? string.Empty);

        return dict;
    }
}