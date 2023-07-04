//  Filename: CustomRegexString.cs
// 
//  Description: A custom attribute to validate string value with a regular expression:
// 
//  Author(s):
//  Arif Matin (arif@nofrixion.com)
// 
//  History:
//  03 Jul 2023  Arif Matin   Created, Harcourt Street, Dublin, Ireland.
// 
//  License:
//  Proprietary NoFrixion.
// -----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NoFrixion.MoneyMoov.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class CustomRegexString : DataTypeAttribute
    {
        public string RegexString { get; set; }

        public CustomRegexString(string regexString) : base(DataType.Text)
        {
            RegexString = regexString;
        }

        public override bool IsValid(object? value)
        {
            Regex matchRegex = new(RegexString);

            if (value == null || string.IsNullOrWhiteSpace((string?)value))
            {
                return true;
            }

            return matchRegex.IsMatch((string)value);
        }
    }
}
