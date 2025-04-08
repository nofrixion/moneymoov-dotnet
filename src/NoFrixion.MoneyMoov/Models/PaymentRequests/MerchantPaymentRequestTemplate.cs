// -----------------------------------------------------------------------------
//  Filename: MerchantPaymentRequestTemplate.cs
// 
//  Description: Merchant payment request template.:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  04 04 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class MerchantPaymentRequestTemplate
{
    public Guid ID { get; set; }
    
    public Guid MerchantID { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }

    public required PaymentRequestTemplate Template { get; set; }
    
    public DateTimeOffset Inserted { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}