using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class NumberInputExamples: ComponentBase
    {

        private decimal _number = 10000.234M;
        private int _intNumber = 3;


        [Inject]
        public IJSRuntime JsRuntime { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JsRuntime.InvokeVoidAsync("Rainbow.color");
        }
    }
}
