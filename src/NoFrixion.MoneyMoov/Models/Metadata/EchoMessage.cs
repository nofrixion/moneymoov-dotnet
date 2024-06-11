// -----------------------------------------------------------------------------
// Filename: EchoMessage.cs
// 
// Description: Model to represent a message that can be "echoed" back from
// the MoneyMoov Metadata API controller.
// 
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 09 Jun 2024  Aaron Clauson   Migrated from main API project.
// 
// License:
// MIT.
// -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models;

public record EchoMessage(string Name, string Message);