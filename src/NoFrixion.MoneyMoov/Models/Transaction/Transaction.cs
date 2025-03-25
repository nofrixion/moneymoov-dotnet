//-----------------------------------------------------------------------------
// Filename: Transaction.cs
//
// Description: The business layer representation of a transaction on the NoFrixion
// MoneyMoov ledger.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 08 Jan 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

#nullable disable

using NoFrixion.MoneyMoov.Extensions;

namespace NoFrixion.MoneyMoov.Models;

public class Transaction : IWebhookPayload, IExportableToCsv
{
    /// <summary>
    /// Unique ID for the transaction.
    /// </summary>
    public Guid ID { get; set; }

    /// <summary>
    /// The ID of the account the transaction belongs to.
    /// </summary>
    public Guid AccountID { get; set; }

    /// <summary>
    /// The name of the account the transaction belongs to.
    /// </summary>
    public string AccountName { get; set; }

    /// <summary>
    /// The ID of the merchant that owns the account.
    /// </summary>
    public Guid MerchantID { get; set; }

    /// <summary>
    /// Type of the transaction.
    /// </summary>
    public TransactionTypesEnum Type { get; set; }

    /// <summary>
    /// Amount of the transaction. Negative values indicate a pay out debit), positive
    /// values a pay in (credit).
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency of transaction.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Description of the transaction.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Date when the transaction occurred.
    /// </summary>
    public DateTimeOffset TransactionDate { get; set; }

    /// <summary>
    /// Date when the transaction was inserted into the ledger.
    /// </summary>
    public DateTimeOffset Inserted { get; set; }

    /// <summary>
    /// Date when the transaction was inserted into the ledger. Note transactions cannot be updated
    /// this property is provided for webhook payload compatability.
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public DateTimeOffset LastUpdated { get; set; }

    /// <summary>
    /// For a pay in the reference the sending party attached. For a pay out the 
    /// reference that the payer attached for themselves.
    /// </summary>
    public string YourReference { get; set; }

    /// <summary>
    /// For a pay out the reference that the payer attached for the receiving party. For a
    /// pay in this will typically be empty but for internal transactions may contain the
    /// reference the sending party set for themselves.
    /// </summary>
    public string TheirReference { get; set; }

    /// <summary>
    /// For a pay in (credit) the counterparty is entity who sent the payment. For a pay out (debit) the 
    /// counterparty is the entity the payment is being made to.
    /// </summary>
    public Counterparty Counterparty { get; set; }

    /// <summary>
    /// For pay in (credit) transactions this will contain a descriptive string with the 
    /// most important fields about the counterparty.
    /// </summary>
    public string CounterpartySummary { get; set; }

    /// <summary>
    /// Balance left on the account after the transaction.
    /// </summary>
    public decimal Balance { get; set; }
    
    /// <summary>
    /// ID of the rule that resulted in the transaction.
    /// </summary>
    public Guid? RuleID { get; set; }

    /// <summary>
    /// ID of the payout that resulted in the transaction.
    /// </summary>
    public Guid? PayoutID { get; set; }

    /// <summary>
    /// If set it indicates the  payin was to a virtual IBAN.
    /// </summary>
    public string VirtualIBAN { get; set; }

    /// <summary>
    /// An optional list of descriptive tags attached to the transaction.
    /// </summary>
    public List<Tag> Tags { get; set; } = [];

    /// <summary>
    /// The sequence number of transaction on a per account basis. This sequence number is guaranteed to be an arithemtic sequence 
    /// number for all transactions belonging to the same account.
    /// </summary>
    public int AccountSequenceNumber { get; set; }

    /// <summary>
    /// For Pay by Bank and Direct Debit transactions this will contain the ID of the payment request.
    /// </summary>
    public Guid? PaymentRequestID { get; set; }

    public string CsvHeader() => "ID,AccountID,AccountName,MerchantID,Type,Amount,Currency,Description,TransactionDate,Inserted,YourReference,TheirReference,Counterparty,Balance,RuleID,PayoutID,VirtualIBAN,Tags,AccountSequenceNumber,PaymentRequestID";
    
    public string ToCsvRow()
    {
        return this.ToCsvRowString();
    }
}
