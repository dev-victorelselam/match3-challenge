using System;
using Context;
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
        [SerializeField] private Button _pause;
        [Space(10)] 
        [SerializeField] private GameMenu _menu;
        [SerializeField] private WinScreen _winScreen;
        [SerializeField] private LoseScreen _loseScreen;
        
        private void Awake()
        {
            _pause.onClick.AddListener(() => ContextProvider.Context.Pause());
            ContextProvider.Context.OnPause.AddListener(Pause);
        }

        private void Pause(bool pause)
        {
            if (pause)
                _menu.Show();
        }

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

        public void ShowWin()
        {
            _winScreen.Show();
        }

        public void ShowLose()
        {
            _loseScreen.Show();
        }
    }
}