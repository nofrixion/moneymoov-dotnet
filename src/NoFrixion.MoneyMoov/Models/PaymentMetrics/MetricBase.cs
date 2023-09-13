// -----------------------------------------------------------------------------
//  Filename: MetricBase.cs
// 
//  Description: Provides a base definition for Metric-related objects.
// 
//  Author(s):
//  Axel Granillo (axel@nofrixion.com)
// 
//  History:
//  31 08 2023  Axel Granillo   Created, Remote, Mexico City, Mexico.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

/// <summary>
/// Represents the different metrics that can be retrieved.
/// </summary>
public enum MetricsEnum
{
    /// <summary>
    /// For use with payment requests and payouts.
    /// </summary>
    All = 0,
    
    /// <summary>
    /// For use with payment requests.
    /// </summary>
    Unpaid = 1,
    
    /// <summary>
    /// For use with PartiallyPaid payment requests.
    /// </summary>
    PartiallyPaid = 2,
    
    /// <summary>
    /// For use with payment requests and payouts.
    /// </summary>
    Paid = 3,
    
    /// <summary>
    /// For use with Authorized payment requests.
    /// </summary>
    Authorized = 4,
    
    /// <summary>
    /// For use with PendingApproval payouts.
    /// </summary>
    PendingApproval = 5,
    
    /// <summary>
    /// For use with InProgress payouts.
    /// </summary>
    InProgress = 6,
    
    /// <summary>
    /// For use with Failed payouts.
    /// </summary>
    Failed = 7
}

/// <summary>
/// Provides a base definition for Metric-related objects.
/// </summary>
public abstract class MetricBase
{
    /// <summary>
    /// The total amounts by status and currency.
    /// </summary>
    public Dictionary<string, Dictionary<string, decimal>> TotalAmountsByCurrency { get; set; } =
        new();
}