using System;
using System.Collections.Generic;
using System.Text;

namespace BBComponents.Models
{
    public class HtmlElementInfo
    {
        public double Top { get; set; }
        public double Left { get; set; }

        public int TopInt => (int)Top;
        public int LeftInt => (int)Left;
    }
}
