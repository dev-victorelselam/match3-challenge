using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers.Game;
using DG.Tweening;
using Domain;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ViewUtils
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Text _timeText;
        [SerializeField] private List<Color> _timerColors;

        private MatchTimer _timer;
        private Dictionary<int, Color> _colorSteps;

        public void SetTimer(MatchTimer timer)
        {
            _colorSteps = GetSteps(timer.Time);
            _timeText.color = _timerColors.Last();
            _timer = timer;
        }

        public void Update()
        {
            if (_timer == null)
                return;
            
            _timeText.text = Extensions.FormatTime(_timer.Time);
            CheckTextColor(_timer.Time, _colorSteps);
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
