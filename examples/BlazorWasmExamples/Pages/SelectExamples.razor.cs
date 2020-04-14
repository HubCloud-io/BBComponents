using BlazorWasmExamples.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class SelectExamples: ComponentBase
    {
        private IEnumerable<Currency> _currencies = Currency.SampleData();

        private int _selectedValue;
    }
}
