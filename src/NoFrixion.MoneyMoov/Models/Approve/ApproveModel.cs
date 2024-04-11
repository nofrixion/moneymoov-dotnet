//-----------------------------------------------------------------------------
// Filename: ApproveModel.cs
// 
// Description: The view model for multi-factor authentication approvals.
// Serves as a template for approving arbitrary data on the identity
// server using strong customer authentication.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 15 Apr 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

using System.Collections.Specialized;
using NoFrixion.MoneyMoov.Enums;

namespace NoFrixion.MoneyMoov.Models.Approve;

public class ApproveModel
{
    public Guid ID { get; set; }

    public ApproveTypesEnum ApproveType { get; set; }

    public string ReturnUrl { get; set; } = string.Empty;

    /// <summary>
    /// OAuth Client ID any resultant tokens will be issued for.
    /// </summary>
    public string ClientID { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public bool HasWebAuthnDevice { get; set; }

    public bool HasOneTimePassword { get; set; }

    public string? CancellationUrl { get; set; }

    public string? Username { get; set; }

    public string? ApproveAction { get; set; }
    
    public string? UserEmail { get; set; }

    public NameValueCollection ToQueryParams()
    {
        return new NameValueCollection
        {
            { nameof(ApproveType), ApproveType.ToString() },
            { nameof(ID), ID.ToString() },
            { nameof(ReturnUrl), ReturnUrl },
            { nameof(ClientID), ClientID },
            { nameof(State), State },
            { nameof(UserEmail), UserEmail }
        };
    }
}