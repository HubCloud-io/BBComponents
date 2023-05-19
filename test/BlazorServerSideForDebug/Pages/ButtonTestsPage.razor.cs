using BBComponents.Enums;
using BBComponents.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorServerSideForDebug.Pages;

public partial class ButtonTestsPage: ComponentBase
{
    [Inject]
    public IAlertService AlertService { get; set; }
    
    private void OnBtnClicked()
    {
        AlertService.Add("Button clicked", BootstrapColors.Info);
    }
}