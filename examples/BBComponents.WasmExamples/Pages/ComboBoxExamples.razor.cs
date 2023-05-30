using BBComponents.WasmExamples.Models;
using Microsoft.AspNetCore.Components;

namespace BBComponents.WasmExamples.Pages
{
    public partial class ComboBoxExamples: ComponentBase
    {
        private List<Product> _products;
        private int _selectedId;

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
                else if (n < 20)
                {
                    title = $"Axe {n}";
                }
                else
                {
                    title = $"Product {n}";
                }

                _products.Add(new Product() { Id = n, Title = title, IsDeleted = isDeleted });
            }
        }
    }
}
