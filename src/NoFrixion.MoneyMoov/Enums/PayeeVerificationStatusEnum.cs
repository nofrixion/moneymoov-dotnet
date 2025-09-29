namespace NoFrixion.MoneyMoov.Enums;

public enum PayeeVerificationStatusEnum
{
    /// <summary>
    /// Indicates that payee verification is not required for this payee.
    /// </summary>
    NotRequired = 0,
    
    /// <summary>
    /// Indicates that payee verification is pending and has not yet been completed.
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// Indicates that payee verification has been successfully completed.
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Indicates that payee verification failed.
    /// </summary>
    Failed = 3
}