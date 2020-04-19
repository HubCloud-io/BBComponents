using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;


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

        [Parameter]
        public EventCallback<TValue> ItemClick { get; set; }

        [Parameter]
        public EventCallback<TValue> ItemDblClick { get; set; }

        [Parameter]
        public RenderFragment<TValue> ItemTemplate { get; set; }

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
