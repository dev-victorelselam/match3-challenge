using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class MatchTimer : IDisposable
    {
        public readonly UnityEvent OnTimeEnd = new UnityEvent();
        public int Time;
        
        public IEnumerator CountDown(int timeInSeconds)
        {
            Time = timeInSeconds;

            while (Time > 0)
            {
                yield return new WaitForSeconds(1);
                Time--;
            }
        
            OnTimeEnd.Invoke();
        }

        public void Dispose()
        {
            OnTimeEnd.RemoveAllListeners();
        }
    }
}