using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Pages
{
    public partial class DatePickerTests: ComponentBase
    {
        private DateTime _lastDay = DateTime.Now.AddDays(-1);
    }
}
