using BBComponents.Enums;
using BBComponents.Models;
using System;

namespace BBComponents.Services
{
    public class AlertService : IAlertService
    {
        public event Action<AlertInstance> OnAlertAdd;

        public AlertService()
        {

        }

        public void Add(string text, BootstrapColors color)
        {
            var newAlert = new AlertInstance(text, color);

            if (OnAlertAdd != null)
            {
                OnAlertAdd(newAlert);
            }
        }

        public void Add(string text, BootstrapColors color, int dismissTimeSeconds)
        {
            var newAlert = new AlertInstance(text, color, dismissTimeSeconds);

            if (OnAlertAdd != null)
            {
                OnAlertAdd(newAlert);
            }
        }
    }
}
