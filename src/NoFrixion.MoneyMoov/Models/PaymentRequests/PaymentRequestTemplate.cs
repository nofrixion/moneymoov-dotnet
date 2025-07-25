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
    
    public PriorityBankOptions PriorityBankOptions { get; set; } = null!;
    
    public CardPaymentAddressOptions CardPaymentAddressOptions { get; set; } = null!;
    
    public CardPaymentCaptureOptions CardPaymentCaptureOptions { get; set; } = null!;
    
    /// <summary>
    /// A list of default fields that are included in the payment request template.
    /// These fields are predefined and map to concrete fields in the payment request.
    /// </summary>
    public List<PaymentRequestTemplateDefaultField>? DefaultFields { get; set; }
    
    /// <summary>
    /// A list of custom fields that can be included in the payment request template.
    /// </summary>
    public List<PaymentRequestTemplateCustomField>? CustomFields { get; set; }
}

/// <summary>
/// This enum defines the type of field in a payment request template.
/// It is one of the predefined types that map to
/// concrete fields in the payment request.
/// </summary>
public enum PaymentRequestDefaultFieldsEnum
{
    None,
    Description,
    Customer,
    DestinationAccount,
    DueDate,
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
}

public class PriorityBankOptions : PaymentOptions
{
    [Obsolete("Use the PriorityBankForEur and PriorityBankForGbp properties instead.")]
    public string? PriorityBank { get; set; }
    public Dictionary<CurrencyTypeEnum, Guid>? PriorityBankIDs { get; set; }
}

public class CardPaymentAddressOptions : PaymentOptions
{
    public bool RequireAddress { get; set; }
}

public class CardPaymentCaptureOptions : PaymentOptions
{
    public bool Automatic { get; set; }
}

/// <summary>
/// This represents a default field in a payment request template.
/// Default fields are predefined fields that map to concrete payment request properties.
/// </summary>
public class PaymentRequestTemplateDefaultField 
    : PaymentRequestTemplateFieldOptions
{
    public required PaymentRequestDefaultFieldsEnum DefaultField { get; set; }
}

public class PaymentRequestTemplateCustomField: PaymentRequestTemplateFieldOptions
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    /// <summary>
    /// The display order of the custom field. The lowest number is displayed first.
    /// This can be used to determine the order in which
    /// the fields are displayed when creating a payment request.
    /// </summary>
    public int DisplayOrder { get; set; }
}

public abstract class PaymentRequestTemplateFieldOptions
{
    public bool DisplayOnHostedPaymentPage { get; set; }
    
    public bool DisplayOnPaymentReceipt { get; set; }
    
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

public class DefaultPaymentRequestTemplate
{
    public static PaymentRequestTemplate Default => new()
    {
        PaymentMethods = new PaymentMethods
        {
            Bank = true,
        },
        PaymentTerms = new PaymentTerms
        {
            AllowPartialPayments = false
        },
        NotificationOptions = new NotificationOptions
        {
            Creator = true,
            UserRoles = null,
            ExternalRecipients = null
        },
        BankPaymentOptions = new BankPaymentOptions
        {
            DestinationAccounts = null,
        },
        PriorityBankOptions = new PriorityBankOptions()
        {
            PriorityBankIDs = null
        },
        CardPaymentAddressOptions = new CardPaymentAddressOptions
        {
            RequireAddress = false
        },
        CardPaymentCaptureOptions = new CardPaymentCaptureOptions
        {
            Automatic = true
        },
        DefaultFields = 
        [
            new PaymentRequestTemplateDefaultField
            {
                Requirement = FieldRequirement.Optional,
                DisplayOnHostedPaymentPage = true,
                DisplayOnPaymentReceipt = true,
                DefaultField = PaymentRequestDefaultFieldsEnum.Description
            },
            new PaymentRequestTemplateDefaultField
            {
                Requirement = FieldRequirement.Optional,
                DefaultField = PaymentRequestDefaultFieldsEnum.Customer,
                DisplayOnPaymentReceipt = true
            },
            new PaymentRequestTemplateDefaultField
            {
                Requirement = FieldRequirement.Hidden,
                DefaultField = PaymentRequestDefaultFieldsEnum.DestinationAccount
            },
            new PaymentRequestTemplateDefaultField
            {
                Requirement = FieldRequirement.Optional,
                DisplayOnHostedPaymentPage = true,
                DefaultField = PaymentRequestDefaultFieldsEnum.DueDate
            }
        ],
        CustomFields = []
    };
}