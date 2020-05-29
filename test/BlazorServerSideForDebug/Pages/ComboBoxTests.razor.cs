using BlazorServerSideForDebug.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace BlazorServerSideForDebug.Pages
{
    public partial class ComboBoxTests : ComponentBase
    {
        private List<Product> _products;
        private int _selectedId;
        private int _selectedId2;
        private List<Tuple<int, string>> _itemsPerPageSource = new List<Tuple<int, string>>();
        private int _itemsPerPage = 15;


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



        }
    }
}
