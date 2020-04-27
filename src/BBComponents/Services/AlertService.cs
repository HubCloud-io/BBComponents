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

        public void AddDismissable(string text, BootstrapColors color, int dismissTimeSeconds = 5)
        {
            var newAlert = new AlertInstance(text, color, dismissTimeSeconds);

            if (OnAlertAdd != null)
            {
                OnAlertAdd(newAlert);
            }
        }
    }
}
