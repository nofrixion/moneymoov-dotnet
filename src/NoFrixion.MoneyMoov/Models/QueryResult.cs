// -----------------------------------------------------------------------------
//  Filename: QueryResult.cs
// 
//  Description: Represents the result of a Get all query.
//  It can contain either a JSON response or a CSV export.

//  Author(s):
//  Saurav Maiti (saurav@nofrixion.com)
// 
//  History:
//  10 Feb 2025  Saurav Maiti   Created, Hamilton gardens, Dublin, Ireland.
// 
//  License:
//  MIT.
// -----------------------------------------------------------------------------

using LanguageExt;
using NoFrixion.MoneyMoov.Enums;
using NoFrixion.MoneyMoov.Models.Export;
using NoFrixion.MoneyMoov.Models.Utils;

namespace NoFrixion.MoneyMoov.Models;

public record QueryResult<T> where T : class
{
    public PageResponse<T>? PagedResourceItems { get; }
    public Option<CsvExportResponse> CsvResult { get; }
    public ContentTypeEnum ContentType { get; }

    private QueryResult(PageResponse<T> resources)
    {
        PagedResourceItems = resources;
        CsvResult = Option<CsvExportResponse>.None;
        ContentType = ContentTypeEnum.Json;
    }

    private QueryResult(Option<CsvExportResponse> csvResult)
    {
        CsvResult = csvResult;
        ContentType = ContentTypeEnum.Csv;
    }

    public static QueryResult<T> FromJson(PageResponse<T> response)
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));

        return new QueryResult<T>(response);
    }

    public static QueryResult<T> FromCsv(Option<CsvExportResponse> csvResult)
    {
        return new QueryResult<T>(csvResult);
    }
}
