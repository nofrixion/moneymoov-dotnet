//-----------------------------------------------------------------------------
// Filename: PaymentRequestMetrics.cs
// 
// Description: Represents payment request metrics for a merchant.
// 
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 21 Mar 2023  Arif Matin   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.PaymentRequests
{
    public enum MetricsEnum
    {
        All = 0,
        Unpaid = 1,
        PartiallyPaid = 2,
        Paid = 3,
        Authorized = 4,
    }
    public class PaymentRequestMetrics
    {
        public PaymentRequestMetrics()
        {
            TotalAmountsByCurrency = new Dictionary<string, Dictionary<string, decimal>>
            {
                { "All", new Dictionary<string, decimal>() },
                { "Paid", new Dictionary<string, decimal>() },
                { "Unpaid", new Dictionary<string, decimal>() },
                { "PartiallyPaid", new Dictionary<string, decimal>() },
                { "Authorized", new Dictionary<string, decimal>() }
            };
        }

        /// <summary>
        /// Total payment request count.
        /// </summary>
        public int All { get; set; }

        /// <summary>
        /// Total payment request count with status None.
        /// </summary>
        public int Unpaid { get; set; }

        /// <summary>
        /// Total payment request count with status PartiallyPaid.
        /// </summary>
        public int PartiallyPaid { get; set; }

        /// <summary>
        /// Total payment request count with status FullyPaid.
        /// </summary>
        public int Paid { get; set; }
        
        /// <summary>
        /// Total payment request count with status Authorized.
        /// </summary>
        public int Authorized { get; set; }
        
        // The below could have been nested with the above 
        // This preserves backwards compatibility with the existing API
        
        /// <summary>
        /// The total amounts by status and currency.
        /// </summary>
        public Dictionary<string, Dictionary<string, decimal>> TotalAmountsByCurrency { get; set; }
    }
}
