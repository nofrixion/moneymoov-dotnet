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
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models;

public class PaymentAccountCreate
{
    /// <summary>
    /// The ID of the merchant the payment account is being created for.
    /// </summary>
    public Guid MerchantID { get; set;}

    /// <summary>
    /// Currency for the account, only EUR, GBP, USD and in sandbox BTC are supported.
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
    /// The payment processor the account should be created with. Different processors
    /// provide different features. Not all payment processors support payment accounts.
    /// An error is returned if an attempt is made to create an account with a non-supported
    /// processor.
    /// </summary>
    [Obsolete("This property is not used. The PaymentProcessor will be set by default.")]
    public PaymentProcessorsEnum PaymentProcessor { get; set; } = PaymentProcessorsEnum.None;

    /// <summary>
    /// For internal use only. Leave empty unless requested otherwise.
    /// </summary>
    public Guid SupplierPhysicalAccountID { get; set; }
    
    /// <summary>
    /// If specified the account type will be set to the specified value
    /// disregarding the merchant default account type.
    /// </summary>
    public AccountTypeEnum? AccountType { get; set; }
    
    /// <summary>
    /// If creating a Tribe account type, then this is the tribe account id
    /// </summary>
    public string? TribeAccountId { get; set; }

    /// <summary>
    /// For EUR accounts this can be set to the ID of another account that will act as the
    /// backing phyiscal account. The new account will then act as a Virtual account, able to 
    /// receive funds but the transactions will be recorded aginst the backing physical account.
    /// </summary>
    public Guid? PhysicalAccountID { get; set; }
    
    /// <summary>
    /// Optional property to indicate whether the account is a trust account.
    /// The account name is displayed on the statement for trust accounts instead of the merchant name.
    /// </summary>
    public bool IsTrustAccount { get; set; }

    /// <summary>
    /// Places all the payment request's properties into a dictionary.
    /// </summary>
    /// <returns>A dictionary with all the payment request's non-collection properties 
    /// represented as key-value pairs.</returns>
    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { nameof(MerchantID), MerchantID.ToString() },
            { nameof(Currency), Currency.ToString() },
            { nameof(AccountName), AccountName ?? string.Empty },
            { nameof(SupplierPhysicalAccountID), SupplierPhysicalAccountID.ToString() },
            { nameof(AccountType), AccountType?.ToString() ?? string.Empty },
            { nameof(TribeAccountId), TribeAccountId ?? string.Empty },
            { nameof(PhysicalAccountID), PhysicalAccountID?.ToString() ?? string.Empty },
            {nameof(IsTrustAccount), IsTrustAccount.ToString()}
        };
    }
}