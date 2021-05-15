using System;
using System.Collections;
using Controllers.Input;
using Controllers.Points;
using Controllers.Sequence;
using Controllers.Sound;
using DG.Tweening;
using Domain;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class GameController : MonoBehaviour
    {
        public readonly UnityEvent<int> OnScoreUpdate = new UnityEvent<int>();
        
        [SerializeField] private Transform _gameGrid;
        [SerializeField] private GameHud _gameHud;
        
        [SerializeField] private AudioClip _swap;
        [SerializeField] private AudioClip _clear;
        
        private IGridPosition[,] _gems;

        private GameSettings _gameSettings;
        private InputController _inputController;
        private LocalStorage.LocalStorage _localStorage;
        private SoundController _soundController;
        private ISequenceChecker _sequenceChecker;
        private IPointsCalculator _pointsCalculator;
        
        private GemSelectionController _gemSelectionController;
        private GridController _gridController;

        public void Initialize(GameSettings gameSettings,
            InputController inputController,
            LocalStorage.LocalStorage localStorage,
            SoundController soundController,
            ISequenceChecker sequenceChecker,
            IPointsCalculator pointsCalculator)
        {
            _gameSettings = gameSettings;
            _inputController = inputController;
            _localStorage = localStorage;
            _soundController = soundController;
            _sequenceChecker = sequenceChecker;
            
            _pointsCalculator = pointsCalculator;
            _gemSelectionController = new GemSelectionController(_inputController);
            _gridController = new GridController(_gameGrid, _gameSettings, sequenceChecker);
            
            _gemSelectionController.OnSelectionComplete.AddListener(
                (first, second) => StartCoroutine(ChangeGemsPosition(first, second)));
            _gemSelectionController.OnSelectionInvalid.AddListener(InvalidMove);
            _gridController.OnSequence.AddListener(CalculatePoints);
        }

        public void StartGame()
        {
            var timer = new MatchTimer();
            var pointsController = new PointsController(_localStorage, _gameSettings, this);
            pointsController.OnGameWin.AddListener(NextLevel);
            
            ResetMatch();
            PopulateGrid();
            
            StartCoroutine(timer.CountDown(_gameSettings.MatchTime));
            timer.OnTimeEnd.AddListener(StartGame);
            
            _gameHud.StartGame(pointsController, timer);
        }
        
        private void ResetMatch()
        {
            _soundController.Play(_clear);
            _gems = _gridController.Clear(_gems);
        }

        private void NextLevel()
        {
            _localStorage.LevelPass();
            StartGame();
        }

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
                    _soundController.Play(_swap);
                    yield return _gridController.ProcessSequence(_gems, sequenceList, sequence);
                    _gems = _gridController.CurrentGrid; //this is not good
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _inputController.Enable(true);
        }

        private void InvalidMove(GemElement first, GemElement second)
        {
            first.InvalidMove();
            second.InvalidMove();
        }
        
        private void CalculatePoints(int sequenceCount, Vector3 position)
        {
            var points = _pointsCalculator.Calculate(sequenceCount, _gameSettings.MinItemsCount,
                _gameSettings.PointsPerItem, _gameSettings.GetMultiplier(sequenceCount));
            OnScoreUpdate.Invoke(points);

            InstantiateNumbers(points, position);
        }

        private void InstantiateNumbers(int count, Vector3 position)
        {
            var pointsUp = Instantiate(_gameSettings.PointsUpPrefab, position, Quaternion.identity);
            pointsUp.text = $"+{count}";
            var newPos = pointsUp.transform.position.y + 2;
            pointsUp.transform.DOMoveY(newPos, 0.7f).SetEase(Ease.Linear)
                .OnComplete(() => Destroy(pointsUp.gameObject));
        }
    }
}