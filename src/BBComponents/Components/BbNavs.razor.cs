using BBComponents.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbNavs<TValue> : ComponentBase where TValue : struct
    {

        private List<Tuple<TValue, string>> _source = new List<Tuple<TValue, string>>();

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
        /// Style of navs: default, pills, tabs.
        /// </summary>
        [Parameter]
        public NavStyles NavStyle { get; set; } = NavStyles.Pills;


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

        protected override void OnInitialized()
        {
            var valueType = typeof(TValue);
            if (valueType == typeof(int)
                || valueType == typeof(byte))
            {
                return;
            }
            throw new NotImplementedException($"BbNavs component does not support type {valueType}");

        }

        protected override void OnParametersSet()
        {

            _source = new List<Tuple<TValue, string>>();


            if (ItemsSource != null)
            {

                foreach (var item in ItemsSource)
                {
                    var propValue = item.GetType().GetProperty(ValueName);
                    var propText = item.GetType().GetProperty(TextName);

                    if (propValue.PropertyType != typeof(TValue))
                    {
                        throw new ArgumentException($"Type of {ValueName} property shoul be {typeof(TValue)}");
                    }

                    var value = (TValue)propValue?.GetValue(item);
                    var text = propText?.GetValue(item)?.ToString();

                    _source.Add(new Tuple<TValue, string>(value, text));


                }
            }


        }

        private async Task OnTabClick(MouseEventArgs e, TValue value)
        {
            Value = value;
            await ValueChanged.InvokeAsync(Value);
        }

        public static string NavsClass(NavStyles navsStyle)
        {
            var resultClass = "";

            switch (navsStyle)
            {
                case NavStyles.Pills:
                    resultClass = "nav-pills";
                    break;
                case NavStyles.Tabs:
                    resultClass = "nav-tabs";
                    break;
            }

            return resultClass;
        }
    }
}
