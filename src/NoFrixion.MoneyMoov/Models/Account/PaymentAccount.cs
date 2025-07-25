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

using JetBrains.Annotations;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Extensions;

namespace NoFrixion.MoneyMoov.Models;

#nullable disable

public class PaymentAccount: IExportableToCsv
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
    /// Balance of the account.
    /// </summary>
    public decimal Balance { get; set; }

    /// <summary>
    /// Balance of the account expressed in the currency’s minor units (e.g. cents, pence).
    /// </summary>
    public long BalanceMinorUnits => Balance.ToAmountMinorUnits(Currency);

    /// <summary>
    /// Total of the payouts that have been submitted for processing.
    /// </summary>
    public decimal SubmittedPayoutsBalance { get; set; }

    /// <summary>
    /// The balance of the submitted payouts expressed in the currency’s minor units (e.g. cents, pence).
    /// </summary>
    public long SubmittedPayoutsBalanceMinorUnits => SubmittedPayoutsBalance.ToAmountMinorUnits(Currency);

    /// <summary>
    /// Total of the payins that are in a pending/review state.
    /// </summary>
    [Obsolete]
    public decimal PendingPayinsBalance { get; set; }
    
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
    /// Is the default account
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// The current available balance of the account. Calculated by subtracting any submitted payments from the current balance.
    /// </summary>
    public decimal AvailableBalance => Balance - SubmittedPayoutsBalance;

    /// <summary>
    /// The available balance expressed in the currency’s minor units (e.g. cents, pence).
    /// </summary>
    public long AvailableBalanceMinorUnits => AvailableBalance.ToAmountMinorUnits(Currency);

    /// <summary>
    /// The payment account supplier name. A payment account can be supplied by multiple payment processors. 
    /// </summary>
    public PaymentProcessorsEnum AccountSupplierName { get; set; }

    /// <summary>
    /// Indicates if the payment account is an externally connected account.
    /// Externally connected account can be used to view account balances and transactions. 
    /// </summary>
    public bool IsConnectedAccount { get; set; }

    /// <summary>
    /// The ID of the consent used to connect the external account.
    /// </summary>
    public Guid? ConsentID { get; set; }

    /// <summary>
    /// The Icon for external accounts
    /// </summary>
    public string ExternalAccountIcon { get; set; }

    /// <summary>
    /// The bank name for external accounts
    /// </summary>
    public string BankName { get; set; }
    
    /// <summary>
    /// The date that the external account will expire
    /// </summary>
    public DateTimeOffset? ExpiryDate { get; set; }
    
    /// <summary>
    /// Indicates if the payment account is connected to a Xero bank feed.
    /// </summary>
    [Obsolete("Please use the XeroBankFeedConnectionStatus property.")]
    public bool IsXeroBankFeed { get; set; }
    
    /// <summary>
    /// Indicates if the payment account is a trust account. The account name is displayed in the statement
    /// instead of the merchant name for trust accounts.
    /// </summary>
    public bool IsTrustAccount { get; set; }
    
    /// <summary>
    /// States the status of the Xero bank feed connection, if applicable.
    /// </summary>
    public XeroBankFeedConnectionStatusEnum? XeroBankFeedConnectionStatus { get; set; }
    
    public XeroBankFeedSyncStatusEnum XeroBankFeedSyncStatus { get; set; }
    
    public DateTimeOffset? XeroBankFeedLastSyncedAt { get; set; }
    
    public DateTimeOffset? XeroBankFeedSyncLastFailedAt { get; set; }
    
    public string XeroBankFeedSyncLastFailureReason { get; set; }
    
    /// <summary>
    /// Indicates the number of unsynchronised transactions with Xero
    /// </summary>
    public int? XeroUnsynchronisedTransactionsCount { get; set; }
    
    /// <summary>
    /// Indicates that the bank feed connection can no longer be found in Xero.
    /// This can mean that the respective account was archived/deleted in Xero.
    /// </summary>
    [Obsolete("Please use the XeroBankFeedConnectionStatus property.")]
    public bool XeroBankFeedConnectionInactive { get; set; }
    
    /// <summary>
    /// The last transaction on the account
    /// </summary>
    [CanBeNull] public LastTransaction LastTransaction { get; set; }
    
    /// <summary>
    /// The user if available that created the account
    /// </summary>
    [CanBeNull]
    public User CreatedBy { get; set; }

    /// <summary>
    /// Either the name of the user, merchant token or api key that created the account 
    /// </summary>
    [CanBeNull] public string CreatedByDisplayName { get; set; }
    
    /// <summary>
    /// Indicates the default payment rail for this account. Normally it will be left as the
    /// default value but in some special cases it may be set to indicate payouts from this account
    /// should be attempted with a specific payment rail.
    /// </summary>
    public PaymentRailEnum DefaultPaymentRail { get; set; }

    /// <summary>
    /// The list of rules associated with this account.
    /// </summary>
    public List<RuleMinimal> Rules { get; set; } = [];

    /// <summary>
    /// Indicates whether the account is archived. Archived accounts are not displayed in the UI.
    /// If an account receives a payment it will be automatically unarchived.
    /// </summary>
    public bool IsArchived { get; set; }

    /// <summary>
    /// Indicates the status of the SEPA Instant payment rail for this account.
    /// </summary>
    public AccountSepaInstantStatusEnum? SupplierSepaInstantStatus { get; set; }

    public string CsvHeader() =>
        "ID,AccountName,AccountSupplierName,IsConnectedAccount,Balance,SubmittedPayoutsBalance,AvailableBalance,Currency,IBAN,SortCode,AccountNumber,Bic,Inserted,LastUpdated,IsDefault,BankName,ExpiryDate,XeroBankFeedConnectionStatus,XeroBankFeedSyncStatus,XeroBankFeedLastSyncedAt,XeroBankFeedSyncLastFailedAt,XeroBankFeedSyncLastFailureReason,XeroUnsynchronisedTransactionsCount,DefaultPaymentRail,IsArchived,SupplierSepaInstantStatus";

    /// <summary>
    /// For virtual accounts this is the ID of the physical account that the virtual account is linked to.
    /// </summary>
    public Guid? PhysicalAccountID { get; set; }
    
    /// <summary>
    /// True if the account is a virtual account.
    /// </summary>
    public bool IsVirtual => PhysicalAccountID != null;
    
    public string ToCsvRow()
    {
        return this.ToCsvRowString();
    }
}