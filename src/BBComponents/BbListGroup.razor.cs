using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BBComponents
{
    public partial class BbListGroup<TValue>: ComponentBase
    {

        private TValue _selectedItem;

        /// <summary>
        /// Classes for HTML element. Optional.
        /// </summary>
        [Parameter]
        public string HtmlClass { get; set; }

        /// <summary>
        /// Style for HTML element. Optional.
        /// </summary>
        [Parameter]
        public string HtmlStyle { get; set; }

        /// <summary>
        /// Colllection to show.
        /// </summary>
        [Parameter]
        public IEnumerable<TValue> ItemsSource { get; set; }

        /// <summary>
        /// Callback for item onclick event. 
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ItemClick { get; set; }

        /// <summary>
        /// Callback for item ondblclick event.
        /// </summary>
        [Parameter]
        public EventCallback<TValue> ItemDblClick { get; set; }

        /// <summary>
        /// Render fragment to display item.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> ItemTemplate { get; set; }

        /// <summary>
        /// Render fragment to display when no data in ItemsSource.
        /// </summary>
        [Parameter]
        public RenderFragment NoDataTemplate { get; set; }

        protected override void OnParametersSet()
        {
            if (ItemsSource.Any())
            {
                _selectedItem = ItemsSource.FirstOrDefault();
            }
        }

        private async Task OnItemClick(MouseEventArgs e, TValue item)
        {
            _selectedItem = item;
            await ItemClick.InvokeAsync(item);
        }

        private async Task OnItemDblClick(MouseEventArgs e, TValue item)
        {
            await ItemDblClick.InvokeAsync(item);
        }

    }
}
