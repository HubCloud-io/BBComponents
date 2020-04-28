using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class FetchData: ComponentBase
    {
        [Inject]
        public BBComponents.Services.IAlertService AlertService { get; set; }

        private void OnAlertSucessAddClick()
        {
            AlertService.Add($"This is a very long message at: {DateTime.Now}. \n And we add one more string", BBComponents.Enums.BootstrapColors.Success);
        }

        private void OnAlertDangerAddClick()
        {
            AlertService.Add($"Error at: {DateTime.Now}", BBComponents.Enums.BootstrapColors.Danger);
        }

        private void OnAlertDismissableClick()
        {
            AlertService.Add($"Dismissable info at: {DateTime.Now}", BBComponents.Enums.BootstrapColors.Info, 3);
        }
    }
}
