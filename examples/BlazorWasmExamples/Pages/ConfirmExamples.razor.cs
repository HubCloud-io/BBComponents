using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmExamples.Pages
{
    public partial class ConfirmExamples: ComponentBase
    {
        private List<string> _results = new List<string>();
        private bool _isConfirmOpen;

        private void OnConfirmShowClick()
        {
            _isConfirmOpen = true;
        }

        private void OnConfirmClose(bool answer)
        {
            _results.Add($"Answer: {answer} at {DateTime.Now}");
            _isConfirmOpen = false;
        }
    }
}
