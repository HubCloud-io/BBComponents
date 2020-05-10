using Microsoft.AspNetCore.Components;
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

    }
}
