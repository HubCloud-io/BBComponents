using BBComponents.Models;
using BBComponents.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Components
{
    /// <summary>
    /// Component to show alerts.
    /// </summary>
    public partial class BbAlertHub : ComponentBase
    {
        private List<AlertInstance> _alerts = new List<AlertInstance>();

        /// <summary>
        /// Service to add alert instances.
        /// </summary>
        [Inject]
        public IAlertService AlertService { get; set; }

        /// <summary>
        /// Top position of alert.
        /// </summary>
        [Parameter]
        public int? Top { get; set; } = 55;

        /// <summary>
        /// Left position of alert.
        /// </summary>
        [Parameter]
        public int? Left { get; set; }

        /// <summary>
        /// Right position of alert.
        /// </summary>
        [Parameter]
        public int? Right { get; set; } = 30;

        /// <summary>
        /// Bottom position of alert.
        /// </summary>
        [Parameter]
        public int? Bottom { get; set; }

        protected override void OnInitialized()
        {
            if (AlertService != null)
            {
                AlertService.OnAlertAdd -= OnAlertAdd;
                AlertService.OnAlertAdd += OnAlertAdd;
            }
        }

        private void OnCloseClick(MouseEventArgs e, AlertInstance alert)
        {

            alert.OnAlertHide -= OnAlertHide;

            _alerts.Remove(alert);
            // this.StateHasChanged();

            InvokeAsync(() => { this.StateHasChanged(); });

        }

        private void OnAlertAdd(AlertInstance alert)
        {
            _alerts.Add(alert);
            this.StateHasChanged();

            if (alert.DismissTimeSeconds > 0)
            {
                alert.OnAlertHide -= OnAlertHide;
                alert.OnAlertHide += OnAlertHide;
            }

        }

        private void OnAlertHide(AlertInstance alert)
        {
            alert.OnAlertHide -= OnAlertHide;

            _alerts.Remove(alert);

            InvokeAsync(() => { this.StateHasChanged(); });

        }

        private string CalcAlertPosition(AlertInstance alert)
        {
            var sb = new StringBuilder();

            var step = 60;
            var alertNumber = _alerts.IndexOf(alert) + 1;


            if (Top.HasValue)
            {
                var top = Top.Value;
                top += (alertNumber - 1) * step;

                sb.Append($"top: {top}px;");

            }
            else if (Bottom.HasValue)
            {
                var bottom = Bottom.Value;
                bottom += (alertNumber - 1) * step;

                sb.Append($"bottom: {bottom}px;");
            }

            if (Left.HasValue)
            {
                sb.Append($"left: {Left.Value}px;");
            }

            if (Right.HasValue)
            {
                sb.Append($"right: {Right.Value}px;");
            }

            return sb.ToString();
        }
    }
}
