using BBComponents.Enums;
using System;

namespace BBComponents.Models
{
    public interface IAlertInstance
    {
        BootstrapColors Color { get; set; }
        int DismissTimeSeconds { get; set; }
        string HtmlClass { get; }
        string Text { get; set; }

        event Action<AlertInstance> OnAlertHide;
    }
}