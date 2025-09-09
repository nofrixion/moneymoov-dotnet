namespace NoFrixion.MoneyMoov.Enums;

public enum PayeeVerificationResultEnum
{
    /// <summary>
    /// Indicates that the payee name verification result is unknown or not provided.
    /// </summary>
    Unknown = 0,
    
    /// <summary>
    /// Indicates that the payee name matches the name provided.
    /// </summary>
    Match = 1,
    
    /// <summary>
    /// Indicates that the payee name does not match the name provided.
    /// </summary>
    NoMatch = 2,
    
    /// <summary>
    /// Indicates that the payee name closely matches the name provided, but there may be minor discrepancies (e.g., spelling variations).
    /// </summary>
    CloseMatch = 3
}