using BBComponents.Abstract;
using BlazorServerSideForDebug.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class ComboBoxHighLoadTests: ComponentBase, IDisposable
    {
        private List<Product> _products = new List<Product>();
        private List<OrderRow> _rows = new List<OrderRow>();

        private IComboBoxDataProvider<int> _dataProvider;
        private int _productId;
        private int _secondProductId = 2;
        private string _secondProductTitle = "Second product";

        private int _itemsCount = 10;
        private int _rowsCount = 10;

        private long _duration;
        private bool _isWaiting;
        private bool disposedValue;

        protected override void OnInitialized()
        {
            _dataProvider = new ProductDataProvider(10000);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Debug.WriteLine($"OnAfterRender: {DateTime.Now}");
          

        }

        private void OnUpdateClick()
        {
            _isWaiting = true;
            this.StateHasChanged();

            GenerateProducts();


            var timer = new Stopwatch();
            timer.Start();

            FillRows();

            timer.Stop();
            _duration = timer.ElapsedMilliseconds;
            _isWaiting = false;

            Debug.WriteLine($"Update done: {DateTime.Now}");
        }

        private void GenerateProducts()
        {
            _products.Clear();

            for (var i = 1; i <= _itemsCount; i++)
            {
                var newItem = new Product()
                {
                    Id = i,
                    Title = $"Product {i}"
                };

                _products.Add(newItem);
            }
        }

        private void FillRows()
        {
            _rows.Clear();
            var rnd = new Random();

            for(var i = 1; i <= _rowsCount; i++)
            {
                var newRow = new OrderRow();
                newRow.Id = i;
                newRow.ProductId = rnd.Next(1, _itemsCount);
                newRow.Amount = 100M;
                

                _rows.Add(newRow);
            }
        }

        private class OrderRow
        {
            public int Id { get; set; }
            public int ProductId { get; set; }
            public decimal Amount { get; set; }
        }

        private void OnSetProductClicked()
        {
            _productId = 25;
        }

        private void OnSetSecondProductClicked()
        {
            _secondProductId = 33;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(_products != null)
                    {
                        _products.Clear();
                    }

                    if(_rows != null)
                    {
                        _rows.Clear();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

     

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
