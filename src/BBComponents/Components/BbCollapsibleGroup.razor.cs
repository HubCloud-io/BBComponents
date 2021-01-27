using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Components
{
    public partial class BbCollapsibleGroup<TValue>: ComponentBase
    {
        private bool _isOpen = true;

        [Parameter]
        public string HtmlClass { get; set; }

        [Parameter]
        public string HtmlStyle { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public TValue DataItem { get; set; }

        [Parameter]
        public bool Open { get; set; }

        [Parameter]
        public RenderFragment<TValue> ChildContent { get; set; }

        protected override void OnInitialized()
        {
            _isOpen = Open;
        }

        private void OnTitleClicked()
        {
            _isOpen = !_isOpen;
        }
    }
}
