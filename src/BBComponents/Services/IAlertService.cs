using BBComponents.Enums;
using BBComponents.Models;
using System;

namespace BBComponents.Services
{
    public interface IAlertService
    {
        event Action<AlertInstance> OnAlertAdd;
        void Add(string text, BootstrapColors color);
        void Add(string text, BootstrapColors color, int dismissTimeSeconds);
    }
}
