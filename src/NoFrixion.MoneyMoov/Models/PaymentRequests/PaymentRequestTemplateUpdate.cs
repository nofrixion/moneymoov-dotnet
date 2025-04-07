// -----------------------------------------------------------------------------
//  Filename: PaymentRequestTemplateUpdate.cs
// 
//  Description: Contains details of a PaymentRequestTemplateUpdate model:
// 
//  Author(s):
//  Donal O'Connor (donal@nofrixion.com)
// 
//  History:
//  07 04 2025  Donal O'Connor   Created, Harcourt St, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests;

public class PaymentRequestTemplateUpdate
{
    public Guid ID { get; set; }
    
    public Guid MerchantID { get; set; }
    
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required PaymentRequestTemplate Template { get; set; }
}