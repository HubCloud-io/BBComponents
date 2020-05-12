using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Models
{
    public class SelectItem<TValue>
    {
        public string Text { get; set; }
        public TValue Value { get; set; }
        public bool IsVisible { get; set; }
        public bool IsDeleted { get; set; }

        public SelectItem()
        {

        }

        public SelectItem(string text, TValue value, bool isDeleted)
        {
            Text = text;
            Value = value;
            IsDeleted = isDeleted;
        }

    }
}
