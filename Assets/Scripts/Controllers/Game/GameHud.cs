using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ViewUtils;

namespace Controllers.Game
{
    public class GameHud : MonoBehaviour
    {
        public UnityEvent OnTimeEnd = new UnityEvent();
        
        [SerializeField] private Text _score;
        [SerializeField] private Timer _time;

        private int _currentScore;
        private GameController _gameController;

        public void StartGame(GameController gameController, MatchTimer timer)
        {
            OnTimeEnd.RemoveAllListeners();
            SetScore(0);
            
            _gameController = gameController;
            _gameController.OnScoreUpdate.AddListener(UpdateScore);
            
            _time.SetTime(timer);
        }

        private void UpdateScore(int score) => SetScore(_currentScore + score);

        private void SetScore(int score)
        {
            _currentScore = score;
            _score.text = $"Score: {score:00}";
        }
    }
}