//-----------------------------------------------------------------------------
// Filename: IMoneyMoovService.cs
//
// Description: Interface for the MoneyMoov service.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 06 Jul 2022  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
//
// License: 
// MIT.
//-----------------------------------------------------------------------------

using LanguageExt;
using NoFrixion.MoneyMoov.Models;

namespace NoFrixion.MoneyMoov.Services;

public interface IMoneyMoovService
{
    Task<Either<NoFrixionProblem, NoFrixionVersion>> VersionAsync();

    Task<Either<NoFrixionProblem, User>> WhoamiAsync();
}
