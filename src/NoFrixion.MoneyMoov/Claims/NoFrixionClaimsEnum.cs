//-----------------------------------------------------------------------------
// Filename: NoFrixionClaimsEnum.cs
//
// Description: Contains an enum with the nofrixion claim types
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 05 Nov 2021  Aaron Clauson  Created, Carmichael House, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Claims;

public enum NoFrixionClaimsEnum
{
    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to approve a payout and that holds the payout ID.
    /// </summary>
    payoutid,

    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to approve a payout and that holds a hash of the critical
    /// payout fields.
    /// </summary>
    payouthash,

    /// <summary>
    /// The name of the claim to add the merchantId to the API token.
    /// </summary>
    merchantid,

    /// <summary>
    /// The name of the claim to add the token id to the API token.
    /// </summary>
    tokenid,

    /// <summary>
    /// Set to true if the authenticated identity is for a NoFrixion compliance officer.
    /// </summary>
    iscompliance,

    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to approve a Rule and that holds the Rule ID.
    /// </summary>
    ruleid,

    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to approve a Rule and that holds a hash of the critical
    /// Rule fields.
    /// </summary>
    rulehash,
    
    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to authroise a beneficiary and that holds the beneficiary ID.
    /// </summary>
    beneficiaryid,

    /// <summary>
    /// The name of the claim to add to strong (SCA) NoFrixion JWT access tokens 
    /// that can be used to authorise a beneficiary and that holds a hash of the critical
    /// beneficiary fields.
    /// </summary>
    beneficiaryhash,

    approvetype,

    approveid,

    approvehash,

    approvekeyid,

    approvesignature,

    approveamr
}
