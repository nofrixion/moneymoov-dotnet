//-----------------------------------------------------------------------------
// Filename: CsvMapper.cs
// 
// Description: Attempts to map a row from a csv file to a MoneyMoov model.
//
// Author(s):
// Aaron Clauson (aaron@nofrixion.com)
// 
// History:
// 30 Jul 2023  Aaron Clauson   Created, Stillorgan Wood, Dublin, Ireland.
// 
// License:
// MIT.
//-----------------------------------------------------------------------------

using CsvHelper;
using System.Globalization;
using NoFrixion.MoneyMoov.Extensions;

namespace NoFrixion.MoneyMoov;

public class CsvMapResult<T> where T : new()
{
    public string CsvRow { get; set; }

    public T Model { get; set; }

    public NoFrixionProblem Problem { get; set; }

    /// <summary>
    /// The CSV row mapped to header, value pairs.
    /// </summary>
    public IDictionary<string, object> CsvMappedRow { get; set; }

    public CsvMapResult()
    {
        CsvRow = string.Empty;
        Model = new T();
        Problem = NoFrixionProblem.Empty;
        CsvMappedRow = new Dictionary<string, object>();
    }
}

public static class CsvMapper
{
    /// <summary>
    /// Attempts to map the rows in the CSV file to a list of MoneyMoov models. The first line in the CSV must be a header row.
    /// </summary>
    /// <typeparam name="T">The type of model the CSV rows should be mapped to.</typeparam>
    /// <param name="reader">The stream reader containing the CSV data.</param>
    /// <param name="mapping">A mapping dictionary that describes how to match the CSV data to each model property.</param>
    /// <returns></returns>
    public static IEnumerable<CsvMapResult<T>> MapToModel<T>(TextReader reader, Dictionary<string, string> mapping) where T : new()
    {
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var result = new CsvMapResult<T>();
                result.CsvRow = csv.Context.Parser.RawRecord;

                var record = csv.GetRecord<dynamic>();

                if (record == null)
                {
                    result.Problem = new NoFrixionProblem("Unable to parse the row as valid csv.");
                }
                else
                {
                    result.CsvMappedRow = (IDictionary<string, object>)record;

                    try
                    {
                        foreach (var property in typeof(T).GetProperties())
                        {
                            if (mapping.ContainsKey(property.Name))
                            {
                                string template = mapping[property.Name];
                                string formattedValue = template.Substitute((IDictionary<string, object>)record);

                                if (property.PropertyType == typeof(Guid))
                                {
                                    if (Guid.TryParse(formattedValue, out Guid guidValue))
                                    {
                                        property.SetValue(result.Model, guidValue);
                                    }
                                }
                                else if (property.PropertyType.IsEnum)
                                {
                                    if (!string.IsNullOrEmpty(formattedValue))
                                    {
                                        var enumValue = Enum.Parse(property.PropertyType, formattedValue, true);
                                        property.SetValue(result.Model, enumValue);
                                    }
                                }
                                else if (property.PropertyType == typeof(List<string>))
                                {
                                    if (!string.IsNullOrEmpty(formattedValue))
                                    {
                                        var stringItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(formattedValue);
                                        property.SetValue(result.Model, stringItems);
                                    }
                                }
                                else
                                {
                                    property.SetValue(result.Model, Convert.ChangeType(formattedValue, property.PropertyType));
                                }
                            }
                        }
                    }
                    catch (Exception excp)
                    {
                        result.Problem = new NoFrixionProblem($"Exception mapping csv row to {typeof(T)}. {excp.Message}");
                    }
                }

                yield return result;
            }
        }
    }
}

