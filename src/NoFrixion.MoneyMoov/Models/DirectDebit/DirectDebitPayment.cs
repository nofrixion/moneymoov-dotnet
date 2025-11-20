// -----------------------------------------------------------------------------
//  Filename: DirectDebitPayment.cs
// 
//  Description: Contains information about a Direct Debit payment attempt for
//               a payment request.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  14 11 2025  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.DirectDebit;

/// <summary>
/// Contains information about a Direct Debit payment attempt for a payment request.
/// </summary>
public class DirectDebitPayment
{
    /// <summary>
    /// The payment processor used for the Direct Debit payment attempt.
    /// </summary>
    public PaymentProcessorsEnum? Processor { get; set; }
    
    /// <summary>
    /// The ID with which the payment processor identifies the payment.
    /// </summary>
    public string? PaymentID { get; set; }
    
    /// <summary>
    /// The reference assigned by the processor to the payment.
    /// </summary>
    public string? PaymentReference { get; set; }
    
    /// <summary>
    /// The last payment request event type for the attempt. Useful for displaying a status.
    /// </summary>
    public PaymentRequestEventTypesEnum? LastEventType { get; set; }
    
    /// <summary>
    /// Date and time in which the payment was created.
    /// </summary>
    public DateTimeOffset? InitiatedAt { get; set; }
    
    /// <summary>
    /// Date and time in which the payment was submitted to the processor.
    /// </summary>
    public DateTimeOffset? SubmittedAt { get; set; }
    
    /// <summary>
    /// Date and time in which the processor successfully completed the payment.
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }
    
    /// <summary>
    /// Date and time of the last failure reported by the processor.
    /// </summary>
    public DateTimeOffset? FailedAt { get; set; }

    /// <summary>
    /// Original date and time requested by the user for the payment to be collected.
    /// Might differ from the actual payment date. See <see cref="CompletedAt"/>. 
    /// </summary>
    public DateTimeOffset? CollectionDate { get; set; }
    
    /// <summary>
    /// The merchant mandate used for the payment attempt.
    /// </summary>
    public Mandates.Mandate? MerchantMandate { get; set; }

    /// <summary>
    /// IBAN of the account that received the payment.
    /// </summary>
    public string? DestinationIBAN { get; set; }
}