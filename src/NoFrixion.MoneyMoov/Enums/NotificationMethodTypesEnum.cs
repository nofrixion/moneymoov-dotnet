// -----------------------------------------------------------------------------
//  Filename: NotificationMethodTypesEnum.cs
// 
//  Description: Types of notification methods that can be used for merchant notifications.
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  11 Jul 2025  Arif Matin     Created, Carrick on Shannon, Lietrim, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums
{
    public enum NotificationMethodTypesEnum
    {
        None = 0,

        /// <summary>
        /// The notification will be sent via webhook.
        /// </summary>
        Webhook = 1,

        /// <summary>
        /// The notification will be sent via email.
        /// </summary>
        Email = 2,
    }
}
