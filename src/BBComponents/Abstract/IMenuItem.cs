using BBComponents.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Abstract
{
    public interface IMenuItem
    {
        Guid Uid { get; set; }
        MenuItemKinds Kind { get; set; }
        string Name { get; set; }
        string Title { get; set; }
        string IconClass { get; set; }
        string HotKeyTooltip { get; set; }
    }
}
