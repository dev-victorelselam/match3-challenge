using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Context;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ViewUtils
{
    public class Timer : MonoBehaviour
    {
        public UnityEvent OnTimeEnd = new UnityEvent();
        [SerializeField] private Text _timeText;
        [SerializeField] private List<Color> _timerColors;
    
        public void SetTime(int timeInSeconds)
        {
            StartCoroutine(CountDown(timeInSeconds));
        }

        private IEnumerator CountDown(int timeInSeconds)
        {
            var time = timeInSeconds;
            var steps = GetSteps(timeInSeconds);
            _timeText.color = _timerColors.Last();
            
            while (time > 0)
            {
                _timeText.text = Extensions.FormatTime(time);
                CheckTextColor(time, steps);
                yield return new WaitForSeconds(1);
                time--;
            }
        
            OnTimeEnd.Invoke();
        }

        private void CheckTextColor(int time, Dictionary<int, Color> colors)
        {
            var color = Color.white;
            foreach (var keyValue in colors)
            {
                if (time <= keyValue.Key)
                {
                    color = keyValue.Value;
                    break;
                }
            }

            if (color != _timeText.color)
            {
                _timeText.color = color;
                _timeText.transform.DOScale(1.1f, 0.3f)
                    .SetEase(Ease.InOutQuad)
                    .SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() =>_timeText.transform.localScale = Vector3.one);
            }
        }

        private Dictionary<int, Color> GetSteps(int timeInSeconds)
        {
            var stepValue = timeInSeconds / _timerColors.Count;
            var steps = new Dictionary<int, Color>();
            for (var i = 1; i <= _timerColors.Count; i++)
                steps.Add(stepValue * i, _timerColors[i - 1]);

            return steps;
        }
    }
}
