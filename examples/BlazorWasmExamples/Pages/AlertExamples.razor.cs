using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class AlertExamples: ComponentBase
    {
        [Inject]
        public BBComponents.Services.IAlertService AlertService { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        private void OnAlertSuccessAddClick()
        {
            AlertService.Add($"Success message at: {DateTime.Now}", BBComponents.Enums.BootstrapColors.Success);
        }

        private void OnAlertDangerAddClick()
        {
            AlertService.Add($"Error at: {DateTime.Now}", BBComponents.Enums.BootstrapColors.Danger);
        }

        private void OnAlertNonDismissableClick()
        {
            AlertService.Add($"Non dismissable info at: {DateTime.Now}", BBComponents.Enums.BootstrapColors.Info, 0);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("Rainbow.color");
        }


    }
}
