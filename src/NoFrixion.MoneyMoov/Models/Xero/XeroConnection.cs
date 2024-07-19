namespace NoFrixion.MoneyMoov.Models.Xero;

#nullable disable
public class XeroConnection
{
    public Guid MerchantID { get; set; }
    
    public DateTimeOffset ConnectedAt { get; set; }
    
    public string TenantName { get; set; }
    
    /// <summary>
    /// This determines whether the real-time connection with Xero is still valid or not.
    /// </summary>
    public bool IsConnectionValid { get; set; }
}