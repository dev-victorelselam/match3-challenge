using System;
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
        private GridController _gridController;

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
            _gridController = new GridController(_gameGrid, _gameSettings, sequenceChecker);
            
            _gemSelectionController.OnSelectionComplete.AddListener(
                (first, second) => StartCoroutine(ChangeGemsPosition(first, second)));
            _gemSelectionController.OnSelectionInvalid.AddListener(
                (first, second) => Debug.LogError($"Invalid!"));
            _gridController.OnSequence.AddListener(CalculatePoints);
        }

        public void StartGame()
        {
            var timer = new MatchTimer();
            
            ResetMatch();
            PopulateGrid();
            
            StartCoroutine(timer.CountDown(_gameSettings.MatchTime));
            _gameHud.StartGame(this, timer);
        }
        
        private void ResetMatch() => _gems = _gridController.Clear(_gems);

        private void PopulateGrid()
        {
            _gameGrid.transform.position = new Vector3(-5, -5);
            _gems = _gridController.Populate();
            
        }
        
        private IEnumerator ChangeGemsPosition(GemElement first, GemElement second)
        {
            var firstSnapshot = new GridPositionSnapshot(first);
            var secondSnapshot = new GridPositionSnapshot(second);

            _inputController.Enable(false);
            
            yield return this.RunAndWait(first.Move(secondSnapshot), second.Move(firstSnapshot));
            _gems = _gridController.UpdateGrid(_gems);

            var sequence = _sequenceChecker.IsMovementValid(_gems, first, second, out var sequenceList);
            switch (sequence)
            {
                case SequenceType.None:
                    //if it is a not valid move, rollback
                    yield return this.RunAndWait(first.Move(firstSnapshot), second.Move(secondSnapshot));
                    break;
                case SequenceType.Horizontal:
                case SequenceType.Vertical:
                    yield return _gridController.ProcessSequence(_gems, sequenceList, sequence);
                    _gems = _gridController.CurrentGrid; //this is not good
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _inputController.Enable(true);
        }
        
        private void CalculatePoints(int sequenceCount, Vector3 position)
        {
            var points = _pointsCalculator.Calculate(sequenceCount, _gameSettings.MinItemsCount,
                _gameSettings.PointsPerItem, _gameSettings.GetMultiplier(sequenceCount));
            OnScoreUpdate.Invoke(points);
        }
    }
}