using BBComponents.Enums;
using BBComponents.Helpers;
using System;
using System.Timers;


namespace BBComponents.Models
{
    public class AlertInstance : IDisposable, IAlertInstance
    {
        private Timer _timer;

        public event Action<AlertInstance> OnAlertHide;

        public string Text { get; set; }
        public BootstrapColors Color { get; set; }
        public int DismissTimeSeconds { get; set; }

        public string HtmlClass => $"alert {HtmlClassBuilder.BuildColorClass("alert", Color)}";

        public AlertInstance()
        {

        }

        public AlertInstance(string text, BootstrapColors color)
        {
            Text = text;
            Color = color;
        }

        public AlertInstance(string text, BootstrapColors color, int dismissTimeSeconds)
        {
            Text = text;
            Color = color;
            DismissTimeSeconds = dismissTimeSeconds;

            if (dismissTimeSeconds > 0)
            {
                _timer = new Timer();
                _timer.Elapsed += new ElapsedEventHandler(OnTimerTick);
                _timer.Interval = dismissTimeSeconds * 1000;
                _timer.Enabled = true;
                _timer.Start();

            }
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {

            _timer.Stop();

            if (OnAlertHide != null)
            {
                OnAlertHide(this);
            }

        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
            }

            OnAlertHide = null;
        }
    }
}
