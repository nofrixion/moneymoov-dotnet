// -----------------------------------------------------------------------------
//  Filename: BeneficiaryCreate.cs
// 
//  Description: Model to create a yapily beneficiary.
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  14 Oct 2025  Arif Matin   Created, Carrick on Shannon, Leitrim, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.OpenBanking
{
    public class BeneficiaryCreate
    {
        /// <summary>
        /// Create a beneficiary for an payment account with it's ID.
        /// </summary>
        public Guid AccountID { get; set; }

        /// <summary>
        /// Only required if creating a beneficiary for a virtual account.
        /// </summary>
        public Guid MerchantID { get; set; }

        /// <summary>
        /// ID of the account identifier to create the beneficiary for a virtual account.
        /// </summary>
        public Guid AccountIdentifierID { get; set; }
    }
}
