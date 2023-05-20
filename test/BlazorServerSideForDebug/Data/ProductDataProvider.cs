using BBComponents.Abstract;
using BBComponents.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Data
{
    public class ProductDataProvider : IComboBoxDataProvider<int>
    {
        private List<Product> _items = new List<Product>();
        private int _itemsCount;

        public ProductDataProvider(int itemsCount)
        {
            _itemsCount = itemsCount;
            GenerateProducts();
        }

        public async Task<List<SelectItem<int>>> GetCollectionAsync()
        {
            var collection = await Task.Run(() => GetCollection());

            return collection;
        }

        public async Task<SelectItem<int>> GetItemAsync(int key)
        {
            var selectItem = await Task.Run(() => GetItem(key));

            return selectItem;
        }

        private List<SelectItem<int>> GetCollection()
        {
            var collection = new List<SelectItem<int>>();
            foreach (var item in _items)
            {
                var selectItem = new SelectItem<int>()
                {
                    Text = item.Title,
                    Value = item.Id
                };
                collection.Add(selectItem);
            }

            return collection;
        }

        private SelectItem<int> GetItem(int key)
        {
            var item = _items.FirstOrDefault(x => x.Id == key);

            if (item == null)
            {
                return null;
            }
            else
            {
                var selectItem = new SelectItem<int>()
                {
                    Text = item.Title,
                    Value = item.Id
                };

                return selectItem;
            }

        }


        private void GenerateProducts()
        {
            _items.Clear();

            for (var i = 1; i <= _itemsCount; i++)
            {
                var newItem = new Product()
                {
                    Id = i,
                    Title = $"Product {i}"
                };

                _items.Add(newItem);
            }
        }

    }
}
