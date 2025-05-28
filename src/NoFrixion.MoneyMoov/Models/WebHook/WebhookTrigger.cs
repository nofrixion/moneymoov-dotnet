//-----------------------------------------------------------------------------
// Filename: WebhookTrigger.cs
// 
// Description: Model to trigger a merchant webhook.
// 
// Author(s):
// Arif Matin (arif@nofrixion.com)
// 
// History:
// 28 May 2025  Arif Matin   Created, Carrick on Shannon, Leitrim, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace NoFrixion.MoneyMoov.Models.WebHook
{
    public class WebhookTrigger
    {
        /// <summary>
        /// The ID of the webhook resource entity. 
        /// This is the ID of the resource that the webhook is being triggered for, such as a transaction or payout.
        /// </summary>
        [Required]
        public Guid EntityID { get; set; }

        /// <summary>
        /// The ID of the merchant that the webhook is for.
        /// </summary>
        [Required]
        public Guid MerchantID { get; set; }

        /// <summary>
        /// The resource type that the webhook should be generated for.
        /// </summary>
        [Required]
        public WebhookResourceTypesEnum ResourceType { get; set; }

        /// <summary>
        /// The type of event the webhook is for, for example created, updated, deleted etc.
        /// </summary>
        [Required]
        public WebhookActionsEnum ActionType { get; set; }
    }
}
