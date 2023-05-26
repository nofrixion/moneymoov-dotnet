//-----------------------------------------------------------------------------
// Filename: WebhookResourcesEnum.cs
// 
// Description: Enum for the different webhook resource types.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 26 May 2023 Aaron Clauson   Created, Harcourt St, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov;

public enum WebhookResourcesEnum
{
    PaymentRequest,

    Payout,

    Rule,

    Transaction
}
