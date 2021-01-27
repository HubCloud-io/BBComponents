using BlazorServerSideForDebug.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class CollapsibleGroupTests: ComponentBase
    {
        private Currency _currency;

        protected override void OnInitialized()
        {
            _currency = new Currency { Id = 1, CodeNumeric = "978", Code = "EUR", Title = "Euro" };
        }
    }
}
