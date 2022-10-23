// -----------------------------------------------------------------------------
//  Filename: CardTokenCreateModes.cs
// 
//  Description: Enum to set mode in which the card token will be created.
//  This will determine whether user consent is required for tokenising card or not.
// 
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  23 10 2022  Saurav Maiti Created, Dorset Street Upper, Dublin, Ireland.
// 
//  License: MIT
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums
{
    public enum CardTokenCreateModes
    {
        /// <summary>
        /// Card will not be tokenised.
        /// </summary>
        None,

        /// <summary>
        /// Card will be tokenised without explicit consent from user.
        /// However, a message making user aware of it will be displayed on the payelement.
        /// </summary>
        ConsentNotRequired,

        /// <summary>
        /// Card will be tokenised only if user consents to it.
        /// </summary>
        UserConsentRequired
    }
}
