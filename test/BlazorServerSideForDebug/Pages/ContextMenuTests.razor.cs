using BBComponents.Abstract;
using BBComponents.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class ContextMenuTests : ComponentBase
    {

        private bool _isCustomMenuOpen;
        private bool _disableGeometryCalculations;
        private double _clientX;
        private double _clientY;

        private List<IMenuItem> _menuItems = new List<IMenuItem>();
        private List<IMenuItem> _selectedItems = new List<IMenuItem>();
        private List<string> _dummy = new List<string>();

        protected override void OnInitialized()
        {

            for(var i = 1; i < 10; i++)
            {
                _menuItems.Add(new MenuItem()
                {
                    Title = $"Option {i}",
                    Name = $"opt{i}",
                    Kind = BBComponents.Enums.MenuItemKinds.Item,
                    IconClass = "fa fa-sync text-primary"
                });

            }

            _menuItems.Add(new MenuItem() { Kind = BBComponents.Enums.MenuItemKinds.Delimiter });

            _menuItems.Add(new MenuItem()
            {
                Title = "Option 2",
                Name = "opt2",
                Kind = BBComponents.Enums.MenuItemKinds.Item,
                IconClass = "fa fa-times text-danger"
            });

            for (var i = 1; i < 10; i++){

                _dummy.Add($"Item {i}");
            }
        }

        private void OnContextMenu(MouseEventArgs e)
        {
            _clientX = e.ClientX;
            _clientY = e.ClientY;

            _isCustomMenuOpen = true;

        }

        private void OnContextMenuClosed(IMenuItem item)
        {
            _selectedItems.Add(item);
            _isCustomMenuOpen = false;

        }
    }
}
