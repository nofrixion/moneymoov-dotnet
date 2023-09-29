// -----------------------------------------------------------------------------
// Filename: PayoutEventTypesEnum.cs
// 
// Description: Enum for the different types of payout events that can
// occur.
//
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 29 Aep 2023  Arif Matin  Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov
{
    public enum PayoutEventTypesEnum
    {
        /// <summary>
        /// Something went wrong and the event type is unknown.
        /// </summary>
        unknown = 0,

        /// <summary>
        /// A payout was authorised by an approver.
        /// </summary>
        authorise = 1,

        /// <summary>
        /// A payout was initiated with the supplier. 
        /// </summary>
        initiate = 2,

        /// <summary>
        /// A payout was initiated. A webhook is sent out when the status of the payment changes.
        /// </summary>
        webhook = 3,

        /// <summary>
        /// A payout was successful and verified through supplier transaction. 
        /// </summary>
        settle = 4,

        /// <summary>
        /// A payout was initiated but the funds failed to settle.
        /// </summary>
        failure = 5,
    }
}
