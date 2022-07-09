//-----------------------------------------------------------------------------
// Filename: TransactionMetadata.cs
//
// Description: ??.
//
// Author(s):
// ??
// 
// History:
// ??   Created, Harcourt St, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class TransactionMetadata
{
    /// <summary>
    /// Amount to be send
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Account balance before executing the transaction
    /// </summary>
    public long AccountBalance { get; set; }

    /// <summary>
    /// Account balance after executing the transaction (considering that is successful). This value should be the `AccountBalance` less the `Amount`.
    /// </summary>
    public decimal BalanceAfterTransaction { get { return AccountBalance - Amount; } }

    /// <summary>
    /// Percentage of the balance to be send (this is not being rounded, it's a double value showing exact precition). Value goes from 0 to 100.
    /// <para>Ex: if the amount of the transaction is 10 and the balance is 200, the `PercentageOfBalanceToTransfer` is 5</para>
    /// <para>Note: if AccountBalance is 0, this value is going to be 100</para>
    /// </summary>
    public decimal PercentageOfBalanceToTransfer => AccountBalance == 0 ? 100 : Amount / AccountBalance * 100;

    /// <summary>
    /// Quantity of transactions sent to any recipient over the last minute (60 seconds) - without considering this one
    /// </summary>
    public int TransactionsOverLastMinute { get; set; }

    /// <summary>
    /// Quantity of transactions sent to any recipient over the last hour (3600 seconds) without considering this one
    /// </summary>
    public int TransactionsOverLastHour { get; set; }

    /// <summary>
    /// Quantity of transactions sent to any recipient over the last 24 hours (86400 seconds) without considering this one
    /// </summary>
    public int TransactionsToday { get; set; }

    /// <summary>
    /// Amount sent to the same recipient (counterparty) over the last minute (60 seconds) - without considering this one
    /// </summary>
    public decimal AmountSentToRecipientOverLastMinute { get; set; }

    /// <summary>
    /// Amount sent to the same recipient (counterparty) over the last hour (3600 seconds) - without considering this one
    /// </summary>
    public decimal AmountSentToRecipientOverLastHour { get; set; }

    /// <summary>
    /// Amount sent to the same recipient (counterparty) over the last 24 hours (86400 seconds) - without considering this one
    /// </summary>
    public decimal AmountSentToRecipientToday { get; set; }

    /// <summary>
    /// Quantity of transactions sent to the same recipient (counterparty) over the last minute (60 seconds) - without considering this one
    /// </summary>
    public int TransactionsToRecipientOverLastMinute { get; set; }

    /// <summary>
    /// Quantity of transactions sent to the same recipient (counterparty) over the last hour (3600 seconds) - without considering this one
    /// </summary>
    public int TransactionsToRecipientOverLastHour { get; set; }

    /// <summary>
    /// Quantity of transactions sent to the same recipient (counterparty) over the last 24 hours (86400 seconds) - without considering this one
    /// </summary>
    public int TransactionsToRecipientToday { get; set; }

    /// <summary>
    /// Amount sent to any recipient over the last minute (60 seconds) - without considering this one
    /// </summary>
    public decimal AmountSpentOverLastMinute { get; set; }

    /// <summary>
    /// Amount sent to any recipient over the last hour (3600 seconds) - without considering this one
    /// </summary>
    public decimal AmountSpentOverLastHour { get; set; }

    /// <summary>
    /// Amount sent to any recipient over the last 24 hours (86400 seconds) - without considering this one
    /// </summary>
    public decimal AmountSpentToday { get; set; }

    /// <summary>
    /// Quantity of low transactions sent to any recipient over the last minute (60 seconds) - without considering this one.
    /// <para>Low transactions are considered those where the amount is equal or below <see cref="Constants.LOW_TRANSACTION_AMOUNT_DEFINITION"/></para>
    /// </summary>
    public int LowTransactionsOverLastMinute { get; set; }

    /// <summary>
    /// Quantity of low transactions sent to any recipient over the last hour (3600 seconds) - without considering this one
    /// <para>Low transactions are considered those where the amount is equal or below <see cref="Constants.LOW_TRANSACTION_AMOUNT_DEFINITION"/></para>
    /// </summary>
    public int LowTransactionsOverLastHour { get; set; }

    /// <summary>
    /// Quantity of low transactions sent to any recipient over the last 24 hours (86400 seconds) - without considering this one
    /// <para>Low transactions are considered those where the amount is equal or below <see cref="Constants.LOW_TRANSACTION_AMOUNT_DEFINITION"/></para>
    /// </summary>
    public int LowTransactionsToday { get; set; }

    /// <summary>
    /// Quantity of round transactions sent over the last minute (60 seconds) - without considering this one
    /// <para>Round transactions are considered those where the amount has not decimal values (ex: 1, 5, 10, 200)</para>
    /// </summary>
    public int RoundTransactionsOverLastMinute { get; set; }

    /// <summary>
    /// Quantity of round transactions sent over the last hour (3600 seconds) - without considering this one
    /// <para>Round transactions are considered those where the amount has not decimal values (ex: 1, 5, 10, 200)</para>
    /// </summary>
    public int RoundTransactionsOverLastHour { get; set; }

    /// <summary>
    /// Quantity of round transactions sent over the last 24 hours (86400 seconds) - without considering this one
    /// <para>Round transactions are considered those where the amount has not decimal values (ex: 1, 5, 10, 200)</para>
    /// </summary>
    public int RoundTransactionsToday { get; set; }

    /// <summary>
    /// Quantity of high round transactions sent over the last minute (60 seconds) - without considering this one
    /// <para>High round transactions are considered those where the amount has not decimal values and are greater or equal to <see cref="Constants.HIGH_ROUND_TRANSACTION_AMOUNT_DEFINITION"/> than (ex: 1000, 1001, 1005, 2000, 5000, 9999)</para>
    /// </summary>
    public int HighRoundTransactionsOverLastMinute { get; set; }

    /// <summary>
    /// Quantity of high round transactions sent over the last hour (3600 seconds) - without considering this one
    /// <para>High round transactions are considered those where the amount has not decimal values and are greater or equal to <see cref="Constants.HIGH_ROUND_TRANSACTION_AMOUNT_DEFINITION"/> than (ex: 1000, 1001, 1005, 2000, 5000, 9999)</para>
    /// </summary>
    public int HighRoundTransactionsOverLastHour { get; set; }

    /// <summary>
    /// Quantity of high round transactions sent over the last 24 hours (86400 seconds) - without considering this one
    /// <para>High round transactions are considered those where the amount has not decimal values and are greater or equal to <see cref="Constants.HIGH_ROUND_TRANSACTION_AMOUNT_DEFINITION"/> than (ex: 1000, 1001, 1005, 2000, 5000, 9999)</para>
    /// </summary>
    public int HighRoundTransactionsToday { get; set; }

    /// <summary>
    /// Country code of the recipient
    /// <para>Ex: IE, GB, ES, FR</para>
    /// </summary>
    public string RecipientCountryCode { get; set; } = "";

    /// <summary>
    /// If the acount sent a transaction to the recipient country before
    /// </summary>
    public bool HadSentTransactionToCountryBefore { get; set; }

    /// <summary>
    /// Average amount ever sent to any recipient
    /// </summary>
    public decimal AverageAmountEverSent { get; set; }

    /// <summary>
    /// Payout ID of the transaction that is being analysed - Not needed to apply any rule, this is to display purposes
    /// </summary>
    public Guid PayoutID { get; set; }

    /// <summary>
    /// Merchant ID which sent the transaction - Not needed to apply any rule, this is to display purposes
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Merchant name which sent the transaction - Not needed to apply any rule, this is to display purposes
    /// </summary>
    public string MerchantName { get; set; } = "";
}