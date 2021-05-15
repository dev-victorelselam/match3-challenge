using Domain;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class PointsController
    {
        public readonly UnityEvent<int> OnPointsUpdated  = new UnityEvent<int>();
        public readonly UnityEvent OnGameWin  = new UnityEvent();
        
        public int Goal { get; }
        private int _currentPoints;
        
        public PointsController(LocalStorage.LocalStorage localStorage, GameSettings gameSettings, GameController gameController)
        {
            var level = localStorage.GetLevel();
            Goal = gameSettings.StartGoal + (gameSettings.GoalStep * level);
            
            gameController.OnScoreUpdate.AddListener(CompareToGoal);
        }

        private void CompareToGoal(int points)
        {
            _currentPoints += points;
            OnPointsUpdated?.Invoke(_currentPoints);
            if (_currentPoints >= Goal)
                OnGameWin.Invoke();
        }
    }
}