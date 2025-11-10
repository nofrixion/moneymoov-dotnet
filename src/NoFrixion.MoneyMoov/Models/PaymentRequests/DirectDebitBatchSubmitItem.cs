//-----------------------------------------------------------------------------
// Filename: DirectDebitBatchSubmitItem.cs
//
// Description: Model for a single direct debit submission item in a batch.
// Each item contains a payment request ID, its associated mandate ID, and
// an optional submit after date for scheduling the direct debit payment.
//
// Author(s):
// Pablo Maldonado (pablo@nofrixion.com)
// 
// History:
// 07 Nov 2025  Pablo Maldonado   Created.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models;

public class DirectDebitBatchSubmitItem
{
    /// <summary>
    /// The ID of the payment request to submit for direct debit.
    /// </summary>
    [Required]
    public Guid PaymentRequestID { get; set; }

    /// <summary>
    /// The ID of the Direct Debit mandate to use for this payment.
    /// </summary>
    [Required]
    public Guid MandateID { get; set; }

    /// <summary>
    /// Optional. Defines when this payment should be earliest submitted
    /// to the customer's bank account. If not specified, payments will be
    /// submitted as soon as possible.
    /// </summary>
    public DateTimeOffset? SubmitAfter { get; set; }
}

