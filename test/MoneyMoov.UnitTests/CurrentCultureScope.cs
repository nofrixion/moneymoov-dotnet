//-----------------------------------------------------------------------------
// Filename: CurrentCultureScope.cs
//
// Description: Restores the current culture after a culture-specific unit test.
//
// Author(s):
// Constantine Nalimov (constantine.nalimov@nofrixion.com)
//
// History:
// 25 Sep 2026  Constantine Nalimov  Created.
//
// License:
// MIT.
//-----------------------------------------------------------------------------

using System.Globalization;

namespace NoFrixion.MoneyMoov.UnitTests;

internal sealed class CurrentCultureScope : IDisposable
{
    private readonly CultureInfo _previousCulture = CultureInfo.CurrentCulture;

    public CurrentCultureScope(string cultureName)
    {
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName);
    }

    public void Dispose()
    {
        CultureInfo.CurrentCulture = _previousCulture;
    }
}
