using BBComponents.Enums;
using BBComponents.Models;
using System;

namespace BBComponents.Services
{
    /// <summary>
    /// Service to add alert instances.
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Event fires whet alert add
        /// </summary>
        event Action<AlertInstance> OnAlertAdd;

        /// <summary>
        /// Add alert. If dismissTimeSeconds > 0 alert will be dismissed after specified interval, 
        /// if dismissTimeSeconds = 0 alert can be close manual only.
        /// </summary>
        /// <param name="text">Alert text</param>
        /// <param name="color">Alert color</param>
        /// <param name="dismissTimeSeconds">Time in seconds after that alert will be dismissed</param>
        void Add(string text, BootstrapColors color, int dismissTimeSeconds = 5);

    }
}
