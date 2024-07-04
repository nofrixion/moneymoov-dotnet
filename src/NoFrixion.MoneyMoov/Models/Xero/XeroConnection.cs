namespace NoFrixion.MoneyMoov.Models.Xero;

#nullable disable
public class XeroConnection
{
    public Guid MerchantID { get; set; }
    
    public DateTimeOffset ConnectedAt { get; set; }
    
    public string XeroTenantName { get; set; }
}