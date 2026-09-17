// -----------------------------------------------------------------------------
//  Filename: BankMessagePresentationModeEnum.cs
//
//  Description: Display modes for bank messages in payment UI.
//
//  Author(s):
//  NoFrixion
//
//  History:
//  17 Sep 2026  NoFrixion  Created.
//
//  License:
//  MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

/// <summary>
/// Defines how bank messaging should be presented to end users.
/// </summary>
public enum BankMessagePresentationModeEnum
{
    /// <summary>
    /// Show the standard message.
    /// </summary>
    Standard = 1,

    /// <summary>
    /// Show warning on tablet/desktop viewports and standard message on mobile.
    /// </summary>
    WarningOnTabletAndDesktop = 2
}
