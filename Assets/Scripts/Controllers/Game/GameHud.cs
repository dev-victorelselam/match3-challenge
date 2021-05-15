using UnityEngine;
using UnityEngine.UI;
using ViewUtils;

namespace Controllers.Game
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] private Text _score;
        [SerializeField] private Text _goal;
        [SerializeField] private Timer _time;

        public void StartGame(PointsController pointsController, MatchTimer timer)
        {
            pointsController.OnPointsUpdated.AddListener(SetScore);

            SetScore(0);
            _goal.text = $"Goal: {pointsController.Goal:00}";
            _time.SetTimer(timer);
        }
        private void SetScore(int score)
        {
            _score.text = $"Score: {score:00}";
        }
    }
}