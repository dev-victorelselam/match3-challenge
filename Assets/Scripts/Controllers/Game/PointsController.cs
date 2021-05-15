using System;
using Domain;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class PointsController : IDisposable
    {
        public readonly UnityEvent<int> OnPointsUpdated  = new UnityEvent<int>();
        public readonly UnityEvent OnGameWin  = new UnityEvent();
        
        public int Goal { get; }
        private int _currentPoints;
        private readonly GameController _gameController;

        public PointsController(LocalStorage.LocalStorage localStorage, GameSettings gameSettings, GameController gameController)
        {
            var level = localStorage.GetLevel();
            Goal = gameSettings.StartGoal + (gameSettings.GoalStep * level);

            _gameController = gameController;
            gameController.OnScoreUpdate.AddListener(CompareToGoal);
        }

        private void CompareToGoal(int points)
        {
            _currentPoints += points;
            OnPointsUpdated?.Invoke(_currentPoints);
            if (_currentPoints >= Goal)
                OnGameWin.Invoke();
        }

        public void Dispose()
        {
            _gameController.OnScoreUpdate.RemoveListener(CompareToGoal);
            OnPointsUpdated.RemoveAllListeners();
        }
    }
}