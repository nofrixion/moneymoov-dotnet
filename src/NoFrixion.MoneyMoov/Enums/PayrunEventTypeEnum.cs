// -----------------------------------------------------------------------------
//   Filename: PayrunEventTypeEnum.cs
// 
//   Description: List of the types of events that can occur in a payrun:
// 
//   Author(s):
//   Donal O'Connor (donal@nofrixion.com)
// 
//   History:
//   27 2 2024  Donal O'Connor   Created, Harcourt Street,
//  Dublin, Ireland.
// 
//   License:
//   Proprietary NoFrixion.
//  -----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Enums;

public enum PayrunEventTypeEnum
{
    None = 0,

    Created = 1,
    
    Authorise = 2,
    
    Submitted = 3,
    
    Rejected = 4,
    
    Edited = 5,
    
    AuthorisationRequested = 6,
    
    Completed = 7,
}