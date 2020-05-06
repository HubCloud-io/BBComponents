using BBComponents.Enums;
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

        private BootstrapModalSizes _size = BootstrapModalSizes.Sm;

        private List<Tuple<BootstrapModalSizes, string>> _availableSizes;

        private bool _isConfirmOpen;


        protected override void OnInitialized()
        {
            _availableSizes = new List<Tuple<BootstrapModalSizes, string>>();

            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Default, "Default"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Sm, "Small"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Lg, "Large"));
            _availableSizes.Add(new Tuple<BootstrapModalSizes, string>(BootstrapModalSizes.Xl, "Extra lagre"));


        }

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

        private void OnConfirmShowClick()
        {
            _isConfirmOpen = true;
        }

        private void OnConfirmClose(bool answer)
        {
            _isConfirmOpen = false;
        }

    }
}
