using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Components
{
    public partial class BbDatePicker: ComponentBase
    {
        private bool _isOpen;

        private void OnOpenClick()
        {
            _isOpen = !_isOpen;
        }

    }
}
