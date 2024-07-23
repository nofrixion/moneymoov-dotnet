namespace NoFrixion.MoneyMoov.Enums;

public enum XeroBankFeedSyncStatusEnum
{
    /// <summary>
    /// Default value
    /// </summary>
    None = 0,
    
    /// <summary>
    /// Xero bank feed sync is in progress
    /// </summary>
    InProgress = 2,
    
    /// <summary>
    /// Xero bank feed sync completed successfully
    /// </summary>
    Completed = 3,
    
    /// <summary>
    /// Xero bank feed sync failed
    /// </summary>
    Failed = 4
}