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
    
    Submitted = 2,
    
    Rejected = 3,
    
    Edited = 4,
    
    AuthorisationRequested = 5,
    
    Completed = 6,
    
    Approved = 7,
}