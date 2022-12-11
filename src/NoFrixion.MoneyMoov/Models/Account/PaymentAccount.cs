﻿// -----------------------------------------------------------------------------
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
    public string IBAN { get; set; }

    /// <summary>
    /// The sort code for GBP accounts. Will be empty for EUR accounts.
    /// </summary>
    public string SortCode { get; set; }

    /// <summary>
    /// The account number for GBP accounts. Will be empty for EUR accounts.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Bank Identification Code.
    /// </summary>
    public string Bic { get; set; }

    /// <summary>
    /// Name for the account
    /// </summary>
    /// <value>Name for the account</value>
    public string AccountName { get; set; }

    /// <summary>
    /// Display name for UI
    /// </summary>
    public string DisplayName => $"{AccountName} ({ID})";
}