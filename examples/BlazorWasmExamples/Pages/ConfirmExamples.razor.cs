using BBComponents.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class ConfirmExamples: ComponentBase
    {
        private List<string> _results = new List<string>();
        private bool _isConfirmOpen;
        private BootstrapModalSizes _size = BootstrapModalSizes.Sm;

        private List<Tuple<BootstrapModalSizes, string>> _availableSizes;

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override void OnInitialized()
        {
            _availableSizes = new List<Tuple<BootstrapModalSizes, string>>();

            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Default, "Default"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Sm, "Small"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Lg, "Large"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Xl, "Extra large"));


        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("Rainbow.color");
        }

        private void OnConfirmShowClick()
        {
            _isConfirmOpen = true;
        }

        private void OnConfirmClose(bool answer)
        {
            _results.Add($"Answer: {answer} at {DateTime.Now}");
            _isConfirmOpen = false;
        }
    }
}
