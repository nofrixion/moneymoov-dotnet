//-----------------------------------------------------------------------------
// Filename: AuthorisationStatus.cs
//
// Description: Represents the authorisation status for a parent model,
// e.g. payout, beneficiary etc. Once the parent model is authorised this state
// can largely be ignored until the next time the parent model is updated.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 02 Aug 2025  Aaron Clauson   Created, Carnesore Point, Wexford, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Approve;

namespace NoFrixion.MoneyMoov.Models;

public class AuthorisationStatus
{
    /// <summary>
    /// A list of users who have successfully authorised the latest version of the parent model.
    /// </summary>
    public List<Authorisation>? Authorisations { get; set; }

    /// <summary>
    /// True if the parent model can be authorised by the user who loaded it.
    /// </summary>
    public bool CanAuthorise { get; set; }

    /// <summary>
    /// True if the parent model can be updated by the user who loaded it.
    /// </summary>
    public bool CanUpdate { get; set; }

    /// <summary>
    /// True if the parent model was loaded for a user and that user has already authorised the latest version.
    /// </summary>
    public bool HasCurrentUserAuthorised { get; set; }

    /// <summary>
    /// The number of authorisers required for the parent model. Is determined by business settings
    /// on the source account and/or merchant.
    /// </summary>
    public int AuthorisersRequiredCount { get; set; }

    /// <summary>
    /// The number of distinct authorisers that have authorised the parent model.
    /// </summary>
    public int AuthorisersCompletedCount { get; set; }

    /// <summary>
    /// A list of authentication types allowed to authorise the parent model.
    /// </summary>
    public List<AuthenticationTypesEnum>? AuthenticationMethods { get; set; }
}
