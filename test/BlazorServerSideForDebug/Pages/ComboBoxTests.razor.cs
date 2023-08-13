using BBComponents.Models;
using BlazorServerSideForDebug.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class ComboBoxTests : ComponentBase
    {
        private List<Product> _products;
        private List<CatalogItem> _fields;
        private string _fieldName = "Id";

        private int _selectedId;
        private string _selectedText;

        private int _selectedId2;
        private string _selectedText2;

        private string _addNewResult;

        private List<Tuple<int, string>> _itemsPerPageSource = new List<Tuple<int, string>>();
        private int _itemsPerPage = 15;

        private List<OrderRow> _orderTable = new List<OrderRow>();

        private ElementReference _tableElementReference;

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        protected override void OnInitialized()
        {
            _products = new List<Product>();

            for (var n = 1; n <= 100; n++)
            {
                var isDeleted = n % 10 == 0;
                var title = $"Product {n}";
                if (n < 10)
                {
                    title = $"Hammer {n}";
                }
                else if ( n < 20)
                {
                    title = $"Axe {n}";
                }
                else
                {
                    title = $"Product {n}";
                }

                _products.Add(new Product() { Id = n, Title = title, IsDeleted = isDeleted });
            }

            _selectedId2 = 3;

            _itemsPerPageSource = new List<Tuple<int, string>>();
            _itemsPerPageSource.Add(new Tuple<int, string>(10, "10"));
            _itemsPerPageSource.Add(new Tuple<int, string>(15, "15"));
            _itemsPerPageSource.Add(new Tuple<int, string>(20, "20"));
            _itemsPerPageSource.Add(new Tuple<int, string>(30, "30"));
            _itemsPerPageSource.Add(new Tuple<int, string>(40, "40"));
            _itemsPerPageSource.Add(new Tuple<int, string>(50, "50"));
            _itemsPerPageSource.Add(new Tuple<int, string>(100, "100"));

            for(var i = 0; i < 21; i++)
            {
                var currentRow = new OrderRow();
                _orderTable.Add(currentRow);
            }

            _fields = new List<CatalogItem>();
            _fields.Add(new CatalogItem(){Name = "id", Title = "Id"});
            _fields.Add(new CatalogItem(){Name = "Uid", Title = "Uid"});
            _fields.Add(new CatalogItem(){Name = "name", Title = "Name"});
            _fields.Add(new CatalogItem(){Name = "Title", Title = "Title"});

        }

        private void OnProductChanged(SelectItem<int> item)
        {
            _selectedText = item.Text;
        }

        private void OnProduct2Changed(SelectItem<int> item)
        {
            _selectedText2 = item.Text;
        }

        private void OnAddNewClicked(ComboBoxAddNewArgs args)
        {
            _addNewResult = args.ToString();
        }

        private async Task OnTestClick()
        {
           var elementInfo = await JsRuntime.InvokeAsync<HtmlElementInfo>("getElementInfo", _tableElementReference);
        }

        private void OnClearValueClick()
        {
            _selectedId2 = 0;
        }

        private void OnSetValueClick()
        {
            _selectedId2 = 7;
        }

      

        private class OrderRow
        {
            public DateTime Period { get; set; }
            public int ProductId { get; set; }
            public decimal Amount { get; set; }
        }

        private void OnOpenClicked(ComboBoxOpenArgs<int> args)
        {
            Debug.WriteLine($"Combo box open clicked: {args}");
        }
    }
}
