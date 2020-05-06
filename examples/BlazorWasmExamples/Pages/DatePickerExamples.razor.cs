using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class DatePickerExamples: ComponentBase
    {
        private DateTime _dateTimeDefault;

        protected override void OnInitialized()
        {
            _dateTimeDefault = DateTime.Now;
        }
    }
}
