//-----------------------------------------------------------------------------
// Filename: MerchantAccountCreate.cs
// 
// Description: Model that can be supplied to the MoneyMoov API to create a mew
// Merchant account.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Dec 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class MerchantAccountCreate
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
}