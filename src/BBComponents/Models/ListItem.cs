using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Models
{
    public class ListItem<TValue>
    {
        public TValue Value { get; set; }
        public bool IsSelected { get; set; }
        public bool IsVisible { get; set; }

        public ListItem()
        {

        }

        public ListItem(TValue value)
        {
            Value = value;
        }
    }
}
