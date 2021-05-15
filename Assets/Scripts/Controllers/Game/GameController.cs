using System.Collections;
using System.Collections.Generic;
using Controllers.Input;
using Controllers.Points;
using Controllers.Sequence;
using Domain;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class GameController : MonoBehaviour
    {
        public UnityEvent<int> OnScoreUpdate = new UnityEvent<int>();
        
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GameHud _gameHud;
        private IGridPosition[,] _gems;

        private GameSettings _gameSettings;
        private ISequenceChecker _sequenceChecker;
        private InputController _inputController;
        private IPointsCalculator _pointsCalculator;
        
        private GemSelectionController _gemSelectionController;
        private GridPopulationController _gridPopulationController;

        public void Initialize(GameSettings gameSettings, 
            ISequenceChecker sequenceChecker,
            InputController inputController,
            IPointsCalculator pointsCalculator)
        {
            _gameSettings = gameSettings;
            _sequenceChecker = sequenceChecker;
            _inputController = inputController;
            _pointsCalculator = pointsCalculator;
            _gemSelectionController = new GemSelectionController(_inputController);
            _gridPopulationController = new GridPopulationController(_gameGrid, _gameSettings, sequenceChecker);
            _gemSelectionController.OnSelectionComplete.AddListener(
                (first, second) => StartCoroutine(ChangeGemsPosition(first, second)));
            _gemSelectionController.OnSelectionInvalid.AddListener(
                (first, second) => Debug.LogError($"Invalid!"));
        }
        
        public void StartGame()
        {
            var timer = new MatchTimer();
            
            ResetMatch();
            PopulateGrid(_gameSettings.GridSettings);
            
            StartCoroutine(timer.CountDown(_gameSettings.MatchTime));
            _gameHud.StartGame(this, timer);
        }

        private void PopulateGrid(GridSettings gridSettings)
        {
            _gems = _gridPopulationController.Populate();
            _gameGrid.transform.position = new Vector3(-gridSettings.Horizontal + 1, gridSettings.Vertical);
        }
        
        private IEnumerator ChangeGemsPosition(GemElement first, GemElement second)
        {
            var firstSnapshot = new GemSnapshot(first);
            var secondSnapshot = new GemSnapshot(second);

            _inputController.Enable(false);
            
            yield return this.RunAndWait(first.Move(secondSnapshot), second.Move(firstSnapshot));
            UpdateGrid();

            //if it is a not valid move, rollback
            if (_sequenceChecker.IsMovementValid(_gems, first, second, out var sequenceList))
                StartCoroutine(CheckForSequences());
            else
                yield return this.RunAndWait(first.Move(firstSnapshot), second.Move(secondSnapshot));

            _inputController.Enable(true);
        }

        private IEnumerator CheckForSequences()
        {
            yield return ProcessSequence(new List<IGridPosition>());
        }
        
        private IEnumerator ProcessSequence(List<IGridPosition> sequence)
        {
            var points = _pointsCalculator.Calculate(sequence.Count, _gameSettings.MinItemsCount,
                _gameSettings.PointsPerItem, _gameSettings.GetMultiplier(sequence.Count));
            
            OnScoreUpdate.Invoke(points);
            yield return null;
            foreach (var gridItem in sequence)
                gridItem.Remove();
        }

        private void UpdateGrid()
        {
            var newGrid = new IGridPosition[_gems.GetLength(0), _gems.GetLength(1)];
            foreach (var gem in _gems)
                newGrid[gem.X, gem.Y] = gem;

            _gems = newGrid;
        }

        private void ResetMatch()
        {
            _gridPopulationController.Clear(_gems);
            _gems = new IGridPosition[0, 0];
        }
    }
}