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

using Newtonsoft.Json;

#nullable disable

namespace NoFrixion.MoneyMoov.Models;

public class Transaction
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
    /// For pay in (credit) transactions this will contain the information about the payer.
    /// </summary>
    [JsonProperty("from")]
    public AccountInfo From { get; set; }

    /// <summary>
    /// For pay in (credit) transactions this will contain a descriptive string with the 
    /// most important fields about the payer. This field is a summary of the data in the 
    /// From field.
    /// </summary>
    public string FromDisplayText { get; set; }

    /// <summary>
    /// For pay out (debit) transactions this will contain the information about the payee.
    /// </summary>
    [JsonProperty("to")]
    public AccountInfo To { get; set; }

    /// <summary>
    /// For pay out (debit) transactions this will contain a descriptive string with the 
    /// most important fields about the payee, or destination for the payment. This field 
    /// is a summary of the data in the To field.
    /// </summary>
    public string ToDisplayText { get; set; }
}
