//-----------------------------------------------------------------------------
// Filename: CsvAttributes.cs
//
// Description: Containes custom attributes used to parse a model into csv
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
namespace NoFrixion.MoneyMoov.Attributes;

using System;

[AttributeUsage(AttributeTargets.All)]
public class CsvColumnAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}

[AttributeUsage(AttributeTargets.All)]
public class CsvIgnoreAttribute : Attribute { }