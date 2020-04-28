using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbConfirm: ComponentBase
    {
        [Parameter]
        public string Title { get; set; } = "Question";

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public string BtnOkText { get; set; } = "OK";

        [Parameter]
        public string BtnCancelText { get; set; } = "Cancel";

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        private async Task OnCancelClick()
        {
            await OnClose.InvokeAsync(false);
        }

        private async Task OnOkClick()
        {
            await OnClose.InvokeAsync(true);
        }
    }
}
