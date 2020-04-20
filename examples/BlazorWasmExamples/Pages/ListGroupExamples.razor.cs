using BlazorWasmExamples.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class ListGroupExamples: ComponentBase
    {
        private List<Currency> _currencies;

        protected override void OnInitialized()
        {
            _currencies = Currency.SampleData().ToList();

        }
    }
}
