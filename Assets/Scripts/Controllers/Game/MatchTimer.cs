using System;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class MatchTimer : IDisposable
    {
        public readonly UnityEvent OnTimeEnd = new UnityEvent();
        public int Time;
        private bool _paused;

        public async void CountDown(int timeInSeconds)
        {
            Time = timeInSeconds;

            while (Time > 0)
            {
                while (_paused)
                    await Task.Delay(10);
                
                await Task.Delay(1000);
                Time--;
            }
        
            OnTimeEnd.Invoke();
        }

        public void Pause(bool paused)
        {
            _paused = paused;
        }

        public void Dispose()
        {
            OnTimeEnd.RemoveAllListeners();
        }
    }
}