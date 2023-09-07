// -----------------------------------------------------------------------------
//  Filename: PayoutMetrics.cs
// 
//  Description: Represents payout metrics for the merchant.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  31 08 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PayoutMetrics : MetricBase
{
    public PayoutMetrics()
    {
        TotalAmountsByCurrency = new Dictionary<string, Dictionary<string, decimal>>()
        {
            { MetricsEnum.All.ToString(), new Dictionary<string, decimal>() },
            { MetricsEnum.InProgress.ToString(), new Dictionary<string, decimal>() },
            { MetricsEnum.PendingApproval.ToString(), new Dictionary<string, decimal>() },
            { MetricsEnum.Failed.ToString(), new Dictionary<string, decimal>() },
            { MetricsEnum.Paid.ToString(), new Dictionary<string, decimal>() },
        };
    }

    /// <summary>
    /// Total payout count.
    /// </summary>
    public decimal All { get; set; }
    
    /// <summary>
    /// Payouts with Pending, Queued or QueuedUpstream status.
    /// </summary>
    public decimal InProgress { get; set; }
    
    /// <summary>
    /// Payouts with PendingApproval or PendingInput status.
    /// </summary>
    public decimal PendingApproval { get; set; }

    /// <summary>
    /// Payouts with Failed, Rejected or Unknown status. 
    /// </summary>
    public decimal Failed { get; set; }
    
    /// <summary>
    /// Payouts with Processed status.
    /// </summary>
    public decimal Paid { get; set; }
}