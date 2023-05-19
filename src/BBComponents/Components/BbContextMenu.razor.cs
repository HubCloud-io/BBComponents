using BBComponents.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbContextMenu : ComponentBase
    {
        private double _windowWidth;
        private double _windowHeight;

        private string _topPx;
        private string _bottomPx;
        private string _leftPx;
        private string _rightPx;

        private bool _isVisible;

        [Parameter]
        public IEnumerable<IMenuItem> Items { get; set; } = new List<IMenuItem>();

        /// <summary>
        /// Width of component in px. Default value 200.
        /// </summary>
        [Parameter]
        public int Width { get; set; } = 200;

        /// <summary>
        /// X position of mouse click.
        /// </summary>
        [Parameter]
        public double ClientX { get; set; }

        /// <summary>
        /// Y position of mouse click.
        /// </summary>
        [Parameter]
        public double ClientY { get; set; }
        
        /// <summary>
        /// If true, no ClientX set to Left, ClientY set to Top.
        /// </summary>
        [Parameter]
        public bool DisableGeometryCalculations { get; set; }

        /// <summary>
        /// Event callback for closed event.
        /// </summary>
        [Parameter]
        public EventCallback<IMenuItem> Closed { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public string WidthPx => $"{Width}px";

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                _windowHeight = await JsRuntime.InvokeAsync<double>("bbComponents.windowHeight");
                _windowWidth = await JsRuntime.InvokeAsync<double>("bbComponents.windowWidth");

            }
            catch (Exception e)
            {
                Debug.WriteLine($"JS call error. Message: {e.Message}");
            }

            CalcGeometry();
        }

        private async Task OnMenuItemClick(IMenuItem item)
        {
            await Closed.InvokeAsync(item);
        }

        private void CalcGeometry()
        {

            if (DisableGeometryCalculations)
            {
                _topPx = $"{(int)ClientY}px";
                _leftPx = $"{(int)ClientX}px";
                _isVisible = true;
                
                return;
            }
            
            if (_windowHeight > 0)
            {
                if (ClientY > _windowHeight / 2)
                {
                    var bottom = (int)(_windowHeight - ClientY);
                    _bottomPx = $"{bottom}px";
                }
                else
                {
                    _topPx = $"{(int)ClientY}px";
                }

            }
            else
            {
                _topPx = $"{(int)ClientY}px";
            }

            if (_windowWidth > 0)
            {
                if (ClientX > (_windowWidth - Width))
                {
                    var right = (int)(ClientX - _windowWidth);
                    if (right < 0)
                    {
                        _rightPx = $"{10}px";
                    }
                    else
                    {
                        _rightPx = $"{right}px";
                    }

                }
                else
                {
                    _leftPx = $"{(int)ClientX}px";
                }


            }
            else
            {
                _leftPx = $"{(int)ClientX}px";
            }

            _isVisible = true;

        }
    }
}
