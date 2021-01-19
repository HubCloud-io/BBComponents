using BBComponents.Abstract;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BBComponents.Components
{
    public partial class BbContextMenu: ComponentBase
    {
        private string WidthPx => $"{Width}px";
        private string Left => $"{(int)ClientX}px";
        private string Top => $"{(int)ClientY}px";

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
        /// Event callback for closed event.
        /// </summary>
        [Parameter]
        public EventCallback<IMenuItem> Closed { get; set; }

        private async Task OnMenuItemClick(IMenuItem item)
        {
            await Closed.InvokeAsync(item);
        }

    }
}
