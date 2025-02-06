//-----------------------------------------------------------------------------
// Filename: CsvExportResponse.cs
//
// Description: A class that's used to store the response of a CSV export.
//
// Author(s):
// Saurav Maiti (saurav@nofrixion.com)
// 
// History:
// 06 Feb 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
//
// License: 
// Proprietary NoFrixion
//-----------------------------------------------------------------------------

namespace NoFrixion.MoneyMoov.Models.Export;

public class CsvExportResponse
{
    const string CSV_CONTENT_TYPE = "text/csv";
    public required byte[] Data { get; set; }
    
    public required string FileName { get; set; }
    
    public string ContentType { get; set; } = CSV_CONTENT_TYPE;
}