using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbCheckBox : ComponentBase
    {
        private bool _value;

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
        public bool Value { get; set; } = default(bool);

        /// <summary>
        /// Tooltip for component.
        /// </summary>
        [Parameter]
        public string Tooltip { get; set; }

        /// <summary>
        /// Event call back for value changed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        /// <summary>
        /// Duplicate event call back for value changed. 
        /// It is necessary to have possibility catch changed even whet we use @bind-Value.
        /// </summary>
        [Parameter]
        public EventCallback<bool> Changed { get; set; }

        protected override void OnParametersSet()
        {
            _value = Value;
        }

        private async Task OnValueChange(ChangeEventArgs e)
        {
            _value = (bool)e.Value;

            await ValueChanged.InvokeAsync(_value);
            await Changed.InvokeAsync(_value);

        }

    }
}
