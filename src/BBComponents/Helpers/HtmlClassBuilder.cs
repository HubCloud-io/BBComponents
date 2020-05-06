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
            var htmlClass = $"{componentName}-{standardColor.ToString().ToLower()}";

            return htmlClass;
        }

        public static string BuildModalSizeClass(BootstrapModalSizes size)
        {
            var htmlClass = size == BootstrapModalSizes.Default ? "" : $"modal-{size.ToString().ToLower()}";

            return htmlClass;
        }

        public static string BuildSizeClass(string elementName, BootstrapElementSizes size)
        {
            var htmlClass = size == BootstrapElementSizes.Default ? "" : $"{elementName}-{size.ToString().ToLower()}";

            return htmlClass;
        }
    }
}
