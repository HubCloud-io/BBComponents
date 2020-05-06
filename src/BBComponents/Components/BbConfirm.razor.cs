using BBComponents.Enums;
using BBComponents.Helpers;
using Microsoft.AspNetCore.Components;
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
        public BootstrapModalSizes Size { get; set; }

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        public string SizeClass => HtmlClassBuilder.BuildModalSizeClass(Size);

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
