using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;

namespace BBComponents.Helpers
{
    public class ValueParser
    {

        public static (TValue Value, bool IsParsed) TryParse<TValue>(string stringValue)
        {
            var nfi = new NumberFormatInfo
            {
                NumberGroupSeparator = " ",
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

            var result = TryParse<TValue>(stringValue, nfi);

            return result;
        }

        public static (TValue Value, bool IsParsed) TryParse<TValue>(string stringValue, NumberFormatInfo nfi)
        {
            var value = default(TValue);
            var valueType = typeof(TValue);

            var isParsed = false;

            if (valueType == typeof(Guid))
            {
                if (Guid.TryParse(stringValue, out var guid))
                {
                    value = (TValue)Convert.ChangeType(guid, typeof(Guid));
                    isParsed = true;
                }
            }
            else if (valueType == typeof(byte))
            {
                if (byte.TryParse(stringValue, NumberStyles.Any, nfi, out var byteValue))
                {
                    value = (TValue)Convert.ChangeType(byteValue, typeof(byte));
                    isParsed = true;
                }
            }
            else if (valueType == typeof(int))
            {
                if (int.TryParse(stringValue, NumberStyles.Any, nfi, out var intValue))
                {
                    value = (TValue)Convert.ChangeType(intValue, typeof(int));
                    isParsed = true;
                }
            }
            else if (valueType == typeof(long))
            {
                if (long.TryParse(stringValue, NumberStyles.Any, nfi, out var longValue))
                {
                    value = (TValue)Convert.ChangeType(longValue, typeof(long));
                    isParsed = true;
                }
            }
            else if (value is decimal)
            {
                if (decimal.TryParse(stringValue, NumberStyles.Any, nfi, out var decimalValue))
                {
                    value = (TValue)Convert.ChangeType(decimalValue, typeof(decimal));
                    isParsed = true;
                }
            }
            else if (value is double)
            {
                if (double.TryParse(stringValue, NumberStyles.Any, nfi, out var doubleValue))
                {
                    value = (TValue)Convert.ChangeType(doubleValue, typeof(double));
                    isParsed = true;
                }

            }
            else if (value is float)
            {
                if (float.TryParse(stringValue, NumberStyles.Any, nfi, out var floatValue))
                {
                    value = (TValue)Convert.ChangeType(floatValue, typeof(float));
                    isParsed = true;
                }

            }
            else if (valueType == typeof(string))
            {
                value = (TValue)Convert.ChangeType(stringValue, typeof(string));
                isParsed = true;
            }
            else if (valueType.IsEnum)
            {
                try
                {
                    var enumValue = Enum.Parse(valueType, stringValue, true);
                    value = (TValue)enumValue;
                    isParsed = true;
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Cannot parse enum value {stringValue}. Message: {e.Message}");
                }
            }

            return (value, isParsed);


        }
    }
}
