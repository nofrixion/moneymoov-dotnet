using System.Globalization;

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestPaymentReceipt
{
    public PaymentRequestPaymentReceipt()
    {
        MerchantLogoUrl = !string.IsNullOrWhiteSpace(MerchantInfo?.ShortName)
            ? $"https://cdn.nofrixion.com/nextgen/assets/merchants/{MerchantInfo.ShortName}/{MerchantInfo.ShortName}.svg"
            : null;
    }

    public required DateTimeOffset GeneratedOn { get; set; }
    
    public string DisplayGeneratedOn => GeneratedOn.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
    
   public required MerchantInfo MerchantInfo { get; set; }
    
    public required decimal TotalAmount { get; set; }
    
    public required CurrencyTypeEnum Currency { get; set; }
    
    public required decimal AmountPaid { get; set; }
    
    public required decimal AmountOutstanding { get; set; }
    
    public required decimal TotalAmountPaid { get; set; }
    
    public required DateTimeOffset PaymentDate { get; set; }
    
    public required string PaymentMethod { get; set; }
    
    public List<PaymentRequestCustomField> CustomFields { get; set; } = [];
    
    public string? CustomerName { get; set; }
    
    public string? MerchantLogoUrl { get; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
}

public class MerchantInfo
{
    public required string Name { get; set; }
    
    public string? ShortName { get; set; }
    
    public string? AddressLine1 { get; set; }
    
    public string? AddressLine2 { get; set; }
    
    public string? PostTown { get; set; }
    
    public string? PostCode { get; set; }
    
    public string? Country { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? FaxNumber { get; set; }
    
    public string? WebsiteAddress { get; set; }
    
    public string? RegistrationNumber { get; set; }
}