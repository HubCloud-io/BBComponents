using Microsoft.AspNetCore.Components;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    /// <summary>
    /// Component to input numbers.
    /// </summary>
    /// <typeparam name="TValue">byte, int, long, float, double, decimal</typeparam>
    public partial class BbNumberInput<TValue> : ComponentBase where TValue : struct
    {
        private string _stringValue;
        private NumberFormatInfo _nfi;

        /// <summary>
        /// Id of HTML input. Optional.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Classes for input. Optional.
        /// </summary>
        [Parameter]
        public string HtmlClass { get; set; }

        /// <summary>
        /// Style for HTML input. Optional.
        /// </summary>
        [Parameter]
        public string HtmlStyle { get; set; }

        /// <summary>
        /// Indicates that component is read-only.
        /// </summary>
        [Parameter]
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Indicates that component is disabled.
        /// </summary>
        [Parameter]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// Numeric value.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; } = default(TValue);

        /// <summary>
        /// Tooltip for component.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Duplicate event call back for value changed. 
        /// It is necessary to have possibility catch changed even whet we use @bind-Value.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> Changed { get; set; }

        /// <summary>
        /// Number of decimal digits in number presentation. Default value is 2.
        /// </summary>
        [Parameter]
        public int Digits { get; set; } = 2;

        /// <summary>
        /// Decimal separator. Default separator is dot ".".
        /// </summary>
        [Parameter]
        public string DecimalSeparator { get; set; } = ".";

        /// <summary>
        /// Group separator. Defatul separator is space " ".
        /// </summary>
        [Parameter]
        public string GroupSeparator { get; set; } = " ";

        protected override void OnInitialized()
        {
            var valueType = Value.GetType();
            if (valueType == typeof(decimal) 
                || valueType == typeof(double) 
                || valueType == typeof(float) 
                || valueType == typeof(int) 
                || valueType == typeof(long) 
                || valueType == typeof(byte))
            {
                return;
            }
            throw new NotImplementedException($"BbNumberInput component does not support type {valueType}");

        }

        protected override void OnParametersSet()
        {
            _nfi = new NumberFormatInfo()
            {
                NumberDecimalDigits = Digits,
                NumberDecimalSeparator = DecimalSeparator,
                NumberGroupSeparator = GroupSeparator

            };
            if (IsIntegerNumberType(Value.GetType()))
            {
                _nfi.NumberDecimalDigits = 0;
            }
            _stringValue = string.Format(_nfi, "{0:N}", Value);
        }

        private async Task OnValueChange(ChangeEventArgs e)
        {
            char? decimalSeparator = null;
            if (_nfi.NumberDecimalDigits > 0)
            {
                decimalSeparator = _nfi.NumberDecimalSeparator[0];
            }
            _stringValue = ToNumericString(e.Value?.ToString(), decimalSeparator);
            StateHasChanged();

            var parseResult = TryParse(_stringValue, _nfi);

            if (parseResult.Item2)
            {
                Value = parseResult.Item1;
                await ValueChanged.InvokeAsync(Value);
                await Changed.InvokeAsync(Value);
            }

        }

        public static Tuple<TValue, bool> TryParse(string stringValue, NumberFormatInfo nfi)
        {
            var value = default(TValue);
            var isParsed = false;

            if (value is decimal)
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
            else if (value is int)
            {
                if (int.TryParse(stringValue, NumberStyles.Integer, nfi, out var intValue))
                {
                    value = (TValue)Convert.ChangeType(intValue, typeof(int));
                    isParsed = true;
                }

            }
            else if (value is long)
            {
                if (long.TryParse(stringValue, NumberStyles.Integer, nfi, out var longValue))
                {
                    value = (TValue)Convert.ChangeType(longValue, typeof(long));
                    isParsed = true;
                }

            }
            else if (value is byte)
            {
                if (byte.TryParse(stringValue, NumberStyles.Integer, nfi, out var byteValue))
                {
                    value = (TValue)Convert.ChangeType(byteValue, typeof(byte));
                    isParsed = true;
                }

            }

            return new Tuple<TValue, bool>(value, isParsed);
        }

        public static string ToNumericString(string initialString, char? decimalSeparator)
        {
            var result = "";

            foreach (char ch in initialString)
            {
                if (char.IsDigit(ch) || ch == '-' )
                {
                    result += ch;
                }
                else if (decimalSeparator.HasValue && ch == decimalSeparator.Value)
                {
                    result += ch;
                }
            }

            return result;
        }

        public bool IsIntegerNumberType(Type type)
        {
            return type == typeof(int) || type == typeof(long) || type == typeof(byte);
        }
    }
}
