//-----------------------------------------------------------------------------
// Filename: DirectDebitBatchSubmitResponse.cs
//
// Description: Response to batch submitting direct debit payments including successful and failed submissions.
// Contains a list of successful submissions and a dictionary of failed submissions keyed by their
// index (1-based) in the original request, allowing the caller to identify which items succeeded or failed.
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
using System.Collections.Generic;

namespace NoFrixion.MoneyMoov.Models;

public class DirectDebitBatchSubmitResponse
{
    /// <summary>
    /// List of successfully submitted direct debit payments.
    /// </summary>
    public List<SuccessfulDirectDebitSubmission> SuccessfulSubmissions { get; set; } = new();

    /// <summary>
    /// Dictionary of failed submissions, keyed by the index (1-based) in the original request.
    /// </summary>
    public Dictionary<int, FailedDirectDebitSubmission> FailedSubmissions { get; set; } = new();
}

public class SuccessfulDirectDebitSubmission
{
    /// <summary>
    /// The ID of the payment request that was successfully submitted.
    /// </summary>
    public Guid PaymentRequestID { get; set; }
}

public class FailedDirectDebitSubmission
{
    /// <summary>
    /// The direct debit batch submit item that failed.
    /// </summary>
    public required DirectDebitBatchSubmitItem SubmissionItem { get; set; }
    
    /// <summary>
    /// The problem details describing why the submission failed.
    /// </summary>
    public required NoFrixionProblem Problem { get; set; }
}

