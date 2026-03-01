using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GameLibrary
{
    public class GameTimer
    {
        private DispatcherTimer timer;
        private TimeSpan remainingTime;
        public event Action TimeUp; 
        public event Action<string> TimeUpdated;
        public GameTimer(int minutes)
        {
            remainingTime = TimeSpan.FromMinutes(minutes);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
        }
        public void Start()
        {
            timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));
            TimeUpdated?.Invoke($"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}");

            if (remainingTime.TotalSeconds <= 0)
            {
                timer.Stop();
                TimeUp?.Invoke(); 
            }
        }
        public void Stop()
        {
            timer.Stop();
        }

    }
}

