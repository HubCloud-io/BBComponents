using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbTextInput : ComponentBase
    {
        private string _value;
        private bool _showMaskDescription;

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
        /// Tooltip for component.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Placeholder for input.
        /// </summary>
        [Parameter]
        public string Placeholder { get; set; }

        /// <summary>
        /// Mask to check string input.
        /// </summary>
        [Parameter]
        public string Mask { get; set; }

        /// <summary>
        /// Error message for wrong text input by mask.
        /// </summary>
        [Parameter]
        public string MaskDescription { get; set; }

        /// <summary>
        /// String value.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Duplicate event call back for value changed. 
        /// It is necessary to have possibility catch changed even whet we use @bind-Value.
        /// </summary>
        [Parameter]
        public EventCallback<string> Changed { get; set; }

        protected override void OnParametersSet()
        {
            _value = Value;
        }

        private async Task OnValueChange(ChangeEventArgs e)
        {
            _value = e.Value?.ToString();

            if (!string.IsNullOrEmpty(Mask))
            {
                var provider = new MaskedTextProvider(Mask);

                _showMaskDescription = false;

                var sb = new StringBuilder();
                var p = 0;
                foreach (var ch in _value)
                {
                    var checkResult = provider.VerifyChar(ch, p, out var charHInt);

                    if (checkResult)
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        _showMaskDescription = true;
                    }

                    p++;
                }

                _value = sb.ToString();
            }

            await ValueChanged.InvokeAsync(_value);
            await Changed.InvokeAsync(_value);
        }

    }
}
