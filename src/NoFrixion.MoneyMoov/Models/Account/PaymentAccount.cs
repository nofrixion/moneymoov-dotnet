// -----------------------------------------------------------------------------
//  Filename: PaymentAccount.cs
// 
//  Description: Represents an electronic money payment account. Typically
//  denominated in EUR or GBP.
//
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  21 10 2021  Donal O'Connor   Created, Carmichael House, Dublin, Ireland.
//  10 12 2022  Aaron Clauson    Adjusted to match the database entity rather than upstream supplier.
//  11 12 2022  Aaron Clauson    Renamed from PaymentAccount to PaymentAccount.
//
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class PaymentAccount
{
    /// <summary>
    /// Unique id for the account.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// The ID of the merchant that owns the account.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Status of the account.
    /// </summary>
    public AccountStatus Status { get; set; }

    /// <summary>
    /// The current available balance of the account. Calculated by subtracting any pending payments from the current balance.
    /// </summary>
    public decimal AvailableBalance { get; set; }

    /// <summary>
    /// Balance of the account.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Timestamp when the account was created.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// Timestamp when the account was last updated.
    /// </summary>
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// Currency of the account in ISO 4217 format
    /// </summary>
    /// <value>Currency of the account in ISO 4217 format</value>
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// The IBAN for EUR accounts. Will be empty for GBP accounts.
    /// </summary>
    [Obsolete("Please use the same named field on the Identifier property.")]
    public string IBAN => Identifier?.IBAN ?? string.Empty;

    /// <summary>
    /// The sort code for GBP accounts. Will be empty for EUR accounts.
    /// </summary>
    [Obsolete("Please use the same named field on the Identifier property.")]
    public string SortCode => Identifier?.SortCode ?? string.Empty;

    /// <summary>
    /// The account number for GBP accounts. Will be empty for EUR accounts.
    /// </summary>
    [Obsolete("Please use the same named field on the Identifier property.")]
    public string AccountNumber => Identifier?.AccountNumber ?? string.Empty;

    /// <summary>
    /// Bank Identification Code.
    /// </summary>
    [Obsolete("Please use the same named field on the Identifier property.")]
    public string Bic => Identifier?.BIC ?? string.Empty;

    /// <summary>
    /// Name for the account
    /// </summary>
    /// <value>Name for the account</value>
    public string AccountName { get; set; }

    /// <summary>
    /// The payment account identifier contains the information needed to access the account
    /// via a payment network.
    /// </summary>
    public AccountIdentifier Identifier { get; set; }

    /// <summary>
    /// Gets a unique display name for the payment account. Useful for when payment accounts need to 
    /// be listed.
    /// </summary>
    public string DisplayName => $"{AccountName} ({ID})";

    /// <summary>
    /// Gets a summary of the payments account's most important properties.
    /// </summary>
    /// <returns></returns>
    public string Summary
        => AccountName + (Identifier != null ? ", " + Identifier.Summary : string.Empty);

    /// <summary>
    /// Flag for default account for a currency. If there are multiple accounts for a currency,
    /// a default account can be selected for that currency.
    /// </summary>
    public bool IsDefault { get; set; }
}