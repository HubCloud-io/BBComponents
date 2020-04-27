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
        /// Add non-dismissable alert
        /// </summary>
        /// <param name="text">Alert text</param>
        /// <param name="color">Alert color</param>
        void Add(string text, BootstrapColors color);

        /// <summary>
        /// Add auto dismissable alert
        /// </summary>
        /// <param name="text">Alert text</param>
        /// <param name="color">Alert color</param>
        /// <param name="dismissTimeSeconds">Time in seconds after that alert will be dismissed</param>
        void Add(string text, BootstrapColors color, int dismissTimeSeconds);
    }
}
