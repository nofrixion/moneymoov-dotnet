//-----------------------------------------------------------------------------
// Filename: AuthenticationTypesEnum.cs
// 
// Description: List of the types of authentication methods that the identity
// server can handle.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 18 Apr 2023  Aaron Clauson   Created, Harcourt Street, Dublin, Ireland.
// 
// License:
// Proprietary NoFrixion.
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

[Flags]
public enum AuthenticationTypesEnum
{
    None = 0,

    WebAuthn = 1,

    /// <summary>
    /// Currently only support TOTP.
    /// </summary>
    OneTimePassword = 2
}