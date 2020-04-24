using System;
using System.Collections.Generic;
using System.Text;
using BBComponents.Enums;

namespace BBComponents.Helpers
{
    public class HtmlClassBuilder
    {
        public static string BuildColorClass(string componentName, BootstrapColors standardColor)
        {
            var cssClass = $"{componentName}-{standardColor.ToString().ToLower()}";

            return cssClass;
        }
    }
}
