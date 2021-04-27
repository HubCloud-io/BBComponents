using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Models
{
    public class ComboBoxOpenArgs<T>
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public T Value { get; set; }

        public override string ToString()
        {
            return $"{Id}:{Text}:{Value}";
        }

    }
}
