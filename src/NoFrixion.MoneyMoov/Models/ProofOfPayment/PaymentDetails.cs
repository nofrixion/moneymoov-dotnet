// -----------------------------------------------------------------------------
//  Filename: PaymentDetails.cs
// 
//  Description: Class containing payment details for a proof of payment document.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  21 11 2024  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

#nullable disable

using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models.ProofOfPayment;

/// <summary>
/// Class containing payment details for a proof of payment document.
/// </summary>
public class PaymentDetails
{
    /// <summary>
    /// Transaction or Payout ID.
    /// </summary>
    public Guid PaymentID { get; set; }
    
    /// <summary>
    /// Payment amount.
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// Indicates if the payment is a payout or a transaction. See <see cref="ProofOfPaymentResourceTypeEnum"/>.
    /// </summary>
    public ProofOfPaymentResourceTypeEnum ResourceType { get; set; }
    
    /// <summary>
    /// NoFrixion's name and address.
    /// </summary>
    public string NoFrixionDetails { get; set; }

    /// <summary>
    /// Currency of the payment.
    /// </summary>
    public CurrencyTypeEnum Currency { get; set; }

    /// <summary>
    /// Name of the payer.
    /// </summary>
    public string PayerName { get; set; }

    /// <summary>
    /// IBAN or account number and sort code of the source account.
    /// </summary>
    public string SourceAccount { get; set; }

    /// <summary>
    /// Name of the recipient.
    /// </summary>
    public string RecipientName { get; set; }

    /// <summary>
    /// IBAN or account number and sort code of the destination account.
    /// </summary>
    public string DestinationAccount { get; set; }

    /// <summary>
    /// Payment reference.
    /// </summary>
    public string Reference { get; set; }

    /// <summary>
    /// List of invoice IDs, separated by commas.
    /// </summary>
    public string InvoiceList { get; set; }

    /// <summary>
    /// Date when the payment was initiated.
    /// </summary>
    public DateTimeOffset PaymentInitializationDate { get; set; }

    /// <summary>
    /// Date when the payment was completed.
    /// </summary>
    public DateTimeOffset PaymentCompletionDate { get; set; }
}