namespace NoFrixion.MoneyMoov.Models.Xero;


#nullable disable
public class XeroToMoneyMoovConnect
{
    public Guid IdentityServerUserID { get; set; }    
    
    public Guid MoneymoovMerchantID { get; set; }
    
    public string Code { get; set; }
}