//-----------------------------------------------------------------------------
// Filename: PaymentRequestMetrics.cs
// 
// Description: Represents payment request metrics for a merchant.
// 
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 21 Mar 2023  Arif Matin   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class PaymentRequestMetrics : MetricBase
{
    public 
        PaymentRequestMetrics()
    {
        TotalAmountsByCurrency = new Dictionary<string, Dictionary<string, decimal>>()
            {
                { MetricsEnum.All.ToString(), new Dictionary<string, decimal>() },
                { MetricsEnum.Paid.ToString(), new Dictionary<string, decimal>() },
                { MetricsEnum.Unpaid.ToString(), new Dictionary<string, decimal>() },
                { MetricsEnum.PartiallyPaid.ToString(), new Dictionary<string, decimal>() },
                { MetricsEnum.Authorized.ToString(), new Dictionary<string, decimal>() }
            };
    }

    /// <summary>
    /// Total payment request count.
    /// </summary>
    public int All { get; set; }

    /// <summary>
    /// Total payment request count with status None.
    /// </summary>
    public int Unpaid { get; set; }

    /// <summary>
    /// Total payment request count with status PartiallyPaid.
    /// </summary>
    public int PartiallyPaid { get; set; }

    /// <summary>
    /// Total payment request count with status FullyPaid.
    /// </summary>
    public int Paid { get; set; }

    /// <summary>
    /// Total payment request count with status Authorized.
    /// </summary>
    public int Authorized { get; set; }
}
