using System;
using System.Collections;
using Context;
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
        private PointsController _pointsController;
        private MatchTimer _timer;
        private bool _paused;

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
            _gridController = gameObject.AddComponent<GridController>();
            _gridController.Initialize(_gameGrid, _gameSettings, sequenceChecker);
            
            _gemSelectionController.OnSelectionComplete.AddListener(
                (first, second) => StartCoroutine(ChangeGemsPosition(first, second)));
            _gemSelectionController.OnSelectionInvalid.AddListener(InvalidMove);
            _gridController.OnSequence.AddListener(CalculatePoints);
            
            ContextProvider.Context.OnPause.AddListener(Pause);
        }

        private void Pause(bool paused)
        {
            _paused = paused;
            
            _inputController.Enable(!_paused);
            _timer.Pause(_paused);
        }

        public void StartGame()
        {
            _timer?.Dispose();
            _timer = new MatchTimer();
            _timer.OnTimeEnd.AddListener(Lose);
            
            _pointsController?.Dispose();
            _pointsController = new PointsController(_localStorage, _gameSettings, this);
            _pointsController.OnGameWin.AddListener(Win);

            Shuffle();

            _timer.CountDown(_gameSettings.MatchTime);
            _gameHud.StartGame(_pointsController, _timer);
        }

        public void Shuffle()
        {
            ResetMatch();
            PopulateGrid();
            _inputController.Enable(true);
        }
        
        private void ResetMatch()
        {
            StopAllCoroutines();
            _soundController.Play(_clear);
            _gems = _gridController.Clear(_gems);
        }

        private void Win()
        {
            _localStorage.LevelPass();
            _gameHud.ShowWin();
        }

        private void Lose()
        {
            _gameHud.ShowLose();
        }

        private void PopulateGrid()
        {
            var gridSettings = _gameSettings.GridSettings;
            Camera.main.transform.position = new Vector3(gridSettings.Horizontal - 1, gridSettings.Vertical, -10);
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
                    if (!_gridController.CheckAvailableGame(_gems))
                    {
                        Shuffle();
                        yield break;
                    }
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