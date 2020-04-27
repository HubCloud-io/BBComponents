using BBComponents.Enums;
using System;

namespace BBComponents.Models
{
    /// <summary>
    /// Describes alert instance
    /// </summary>
    public interface IAlertInstance
    {
        /// <summary>
        /// Event fires when alert hide
        /// </summary>
        event Action<AlertInstance> OnAlertHide;

        /// <summary>
        /// Alert text
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Alert color
        /// </summary>
        BootstrapColors Color { get; set; }

        /// <summary>
        /// Time in seconds after that alert will be dismissed
        /// </summary>
        int DismissTimeSeconds { get; set; }

        /// <summary>
        /// Html class of alert
        /// </summary>
        string HtmlClass { get; }

    }
}