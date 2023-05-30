using BBComponents.WasmExamples.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BBComponents.WasmExamples.Pages
{
    public partial class SelectExamples: ComponentBase
    {
        private IEnumerable<Currency> _currencies = Currency.SampleData();
        private int _selectedValue;

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("Rainbow.color");
        }
    }
}
