// -----------------------------------------------------------------------------
// Filename: NoFrixionVersion.cs
// 
// Description: Model to represent the current version of the NoFrixion 
// API.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// Valentines Day 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public class NoFrixionVersion
{
    public int MajorVersion { get; }
    
    public int MinorVersion { get; }
    
    public int BuildVersion { get; }

    public string ReleaseName { get; }

    public NoFrixionVersion(int majorVersion, int minorVersion, int buildVersion, string releaseName)
    {
        MajorVersion = majorVersion;
        MinorVersion = minorVersion;
        BuildVersion = buildVersion;
        ReleaseName = releaseName;
    }

    public override string ToString()
    {
        return $"{MajorVersion}.{MinorVersion}.{BuildVersion} - {ReleaseName}";
    }
}
