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

    approveamr,

    /// <summary>
    /// Set to true if the authenticated identity is for a NoFrixion operations officer.
    /// </summary>
    isoperations,
    
    two_factor_enabled,
    
    passkey_added,
    
    verfied_by_api_key,
    
    appid,
    
    use_standard_user_roles,
    
    use_permissions,

    /// <summary>
    /// If set indicates the request was authenticated with a JWT bearer token.
    /// </summary>
    merchant_token_bearer,

    /// <summary>
    /// If set indicates the request was was received from a source address on the merchant token's IP address whitelist.
    /// </summary>
    merchant_token_whitelisted_ipaddress,

    /// <summary>
    /// If set indicates the request was authenticated by a signed (HMAC or public key) merchant token.
    /// </summary>
    merchant_token_signed,

    /// <summary>
    /// Set to true if the authenticated identity is for a NoFrixion operations manager.
    /// </summary>
    isoperationsmanager,
}
