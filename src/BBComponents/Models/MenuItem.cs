using BBComponents.Abstract;
using BBComponents.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Models
{
    public class MenuItem: IMenuItem
    {
        public Guid Uid { get; set; }
        public MenuItemKinds Kind { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string IconClass { get; set; }
    }
}
