//-----------------------------------------------------------------------------
// Filename: MerchantMinimal.cs
// 
// Description: A minimal model for a MoneyMoov merchant.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 23 Jan 2025     Created, Belvedere Road, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models
{
    public class MerchantMinimal
    {
        /// <summary>
        /// Unique ID for the merchant.
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// The registered business name of the merchant.
        /// </summary>
        public string? Name { get; set; }
    }
}
