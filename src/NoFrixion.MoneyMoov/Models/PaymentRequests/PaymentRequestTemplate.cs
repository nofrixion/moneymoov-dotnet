// -----------------------------------------------------------------------------
//  Filename: PaymentRequestTemplate.cs
// 
//  Description: Payment request template.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  28 03 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestTemplate
{
    public PaymentMethods PaymentMethods { get; set; } = null!;

    public PaymentTerms PaymentTerms { get; set; } = null!;
    
    public NotificationOptions NotificationOptions { get; set; } = null!;
    
    public BankPaymentOptions BankPaymentOptions { get; set; } = null!;
    
    public CardPaymentAddressOptions CardPaymentAddressOptions { get; set; } = null!;
    
    public CardPaymentCaptureOptions CardPaymentCaptureOptions { get; set; } = null!;
    
    public List<PaymentRequestTemplateField> Fields { get; set; } = null!;
}

public class PaymentMethods : PaymentOptions
{
    public bool Bank { get; set; }
    
    public bool Card { get; set; }
    
    public bool Apple { get; set; }
    
    public bool Google { get; set; }
}

public class PaymentTerms : PaymentOptions
{
    public bool AllowPartialPayments { get; set; }
}

public class NotificationOptions : PaymentOptions
{
    public bool Creator { get; set; }
    
    public List<Guid>? UserRoles { get; set; }

    public List<string>? ExternalRecipients { get; set; }
}

public class BankPaymentOptions : PaymentOptions
{
    public Dictionary<CurrencyTypeEnum, Guid>? DestinationAccounts { get; set; }
    
    public string? PriorityBank { get; set; }
}

public class CardPaymentAddressOptions : PaymentOptions
{
    public bool RequireAddress { get; set; }
}

public class CardPaymentCaptureOptions : PaymentOptions
{
    public bool Automatic { get; set; }
}

public class PaymentRequestTemplateField
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }

    public bool DisplayForPayer { get; set; }

    public FieldRequirement Requirement { get; set; }
}

public abstract class PaymentOptions
{
    public bool AllowOverride { get; set; }
}

public enum FieldRequirement
{
    Optional,
    Required,
    Hidden
}