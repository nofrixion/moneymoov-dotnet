// -----------------------------------------------------------------------------
//  Filename: IExportableToCsv.cs
// 
//  Description: Interface for entities that can be exported to CSV.
//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  13 Feb 2025  Saurav Maiti  Created, Harcourt Steet, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

namespace NoFrixion.Common.Models.ResourceExport;

public interface IExportableToCsv
{
    string CsvHeader { get; }
    
    string ToCsvRow();
}