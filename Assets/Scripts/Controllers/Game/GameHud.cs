using UnityEngine;
using UnityEngine.UI;
using ViewUtils;

namespace Controllers.Game
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private Text _score;
        [SerializeField] private Timer _time;

        public void StartGame(int timeInSeconds)
        {
            _time.SetTime(timeInSeconds);
            SetScore(0);
        }

        private void SetScore(int score)
        {
            _score.text = $"Score: {score:00}";
        }
    }
}