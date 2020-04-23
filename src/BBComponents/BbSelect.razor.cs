using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents
{
    /// <summary>
    /// Select component.
    /// </summary>
    /// <typeparam name="TValue">byte, int, long, Guid, string</typeparam>
    public partial class BbSelect<TValue>: ComponentBase
    {

        private List<Tuple<string, string>> _source = new List<Tuple<string, string>>();

        private string _value;


        /// <summary>
        /// Id of HTML element. Optional.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// Name of HTML element. Optional.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Classes for HTML element. Optional.
        /// </summary>
        [Parameter]
        public string HtmlClass { get; set; }

        /// <summary>
        /// Style for HTML element. Optional.
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
        /// Selected value.
        /// </summary>
        [Parameter]
        public TValue Value { get; set; } = default(TValue);

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ValueChanged { get; set; }

        /// <summary>
        /// Colllection for select options.
        /// </summary>
        [Parameter]
        public IEnumerable<object> ItemsSource { get; set; }

        /// <summary>
        /// Name of property with option Value.
        /// </summary>
        [Parameter]
        public string ValueName { get; set; }

        /// <summary>
        /// Name of property with option Text.
        /// </summary>
        [Parameter]
        public string TextName { get; set; }

        /// <summary>
        /// When true add option with empty Value and text from TextNotSelectedItem parameter.
        /// </summary>
        [Parameter]
        public bool AddNotSelectedItem { get; set; }

        /// <summary>
        /// Text for option with empty value.
        /// </summary>
        [Parameter]
        public string TextNotSelectedItem { get; set; }

        protected override void OnInitialized()
        {
            var valueType = typeof(TValue);
            if (valueType == typeof(Guid)
                || valueType == typeof(int)
                || valueType == typeof(long)
                || valueType == typeof(byte)
                || valueType == typeof(string))
            {
                return;
            }
            throw new NotImplementedException($"BbSelect component does not support type {valueType}");

        }

        protected override void OnParametersSet()
        {

            _source = new List<Tuple<string, string>>();

            if (AddNotSelectedItem)
            {
                var notSelectedText = string.IsNullOrEmpty(TextNotSelectedItem) ? "== Not selected ==" : TextNotSelectedItem;
                _source.Add(new Tuple<string, string>(null, notSelectedText));
            }

            if (ItemsSource != null)
            {

                foreach (var item in ItemsSource)
                {
                    var propValue = item.GetType().GetProperty(ValueName);
                    var propText = item.GetType().GetProperty(TextName);

                    var value = propValue?.GetValue(item)?.ToString();
                    var text = propText?.GetValue(item)?.ToString();

                    _source.Add(new Tuple<string, string>(value, text));
                }
            }

            _value = Value.ToString();

        }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            _value = e.Value?.ToString();

            TValue result = default(TValue);

            var parseResult = TryParse(_value);

            if (parseResult.Item2)
            {
                await ValueChanged.InvokeAsync(result);
            }

        }

        public static Tuple<TValue, bool> TryParse(string stringValue)
        {
            var value = default(TValue);
            var valueType = typeof(TValue);
            var isParsed = false;

            if (valueType == typeof(Guid))
            {
                if (Guid.TryParse(stringValue, out var guid))
                {
                    value = (TValue)Convert.ChangeType(guid, typeof(Guid));
                }
            }
            else if (valueType == typeof(byte))
            {
                if (byte.TryParse(stringValue, out var byteValue))
                {
                    value = (TValue)Convert.ChangeType(byteValue, typeof(byte));
                }
            }
            else if (valueType == typeof(int))
            {
                if (int.TryParse(stringValue, out var intValue))
                {
                    value = (TValue)Convert.ChangeType(intValue, typeof(int));
                }
            }
            else if (valueType == typeof(long))
            {
                if (long.TryParse(stringValue, out var longValue))
                {
                    value = (TValue)Convert.ChangeType(longValue, typeof(long));
                }
            }
            else if (valueType == typeof(string))
            {
                value = (TValue)Convert.ChangeType(stringValue, typeof(string));
            }

            return new Tuple<TValue, bool>(value, isParsed);


        }

    }
}
