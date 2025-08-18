// -----------------------------------------------------------------------------
//  Filename: PaymentAccountMinimal.cs
// 
//  Description: A minimal representation of a payment account:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  24 07 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------
#nullable disable

namespace NoFrixion.MoneyMoov.Models;

public class PaymentAccountMinimal
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
    /// Currency of the account in ISO 4217 format
    /// </summary>
    /// <value>Currency of the account in ISO 4217 format</value>
    public CurrencyTypeEnum Currency { get; set; }
    
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
    /// Is the account archived
    /// </summary>
    public bool IsArchived { get; set; }
    
    /// <summary>
    /// Indicates if the payment account is an externally connected account.
    /// Externally connected account can be used to view account balances and transactions. 
    /// </summary>
    public bool IsConnectedAccount { get; set; }
    
    /// <summary>
    /// Total of the payouts that have been submitted for processing.
    /// </summary>
    public decimal SubmittedPayoutsBalance { get; set; }
    
    /// <summary>
    /// The current available balance of the account. Calculated by subtracting any submitted payments from the current balance.
    /// </summary>
    public decimal AvailableBalance => Balance - SubmittedPayoutsBalance;
}