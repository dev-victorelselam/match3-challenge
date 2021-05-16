using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers.Sequence;
using Domain;
using UnityEngine;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class GridController : MonoBehaviour
    {
        public readonly UnityEvent<int, Vector3> OnSequence = new UnityEvent<int, Vector3>();
        private const float DelayBetweenProcess = 0.8f;
        
        private Transform _container;
        private GameSettings _gameSettings;
        private ISequenceChecker _sequenceChecker;

        private IGridPosition[,] _currentGrid;
        private Coroutine _repopulateGridRoutine;
        public IGridPosition[,] CurrentGrid => _currentGrid;

        public void Initialize(Transform container, GameSettings gameSettings, ISequenceChecker sequenceChecker)
        {
            _container = container;
            _gameSettings = gameSettings;
            _sequenceChecker = sequenceChecker;
        }

        public IGridPosition[,] Populate()
        {
            var grid = new IGridPosition[_gameSettings.GridSettings.Horizontal, _gameSettings.GridSettings.Vertical];
            for (var i = 0; i < _gameSettings.GridSettings.Horizontal; i++)
            for (var j = 0; j < _gameSettings.GridSettings.Vertical; j++)
            {
                var gem = _gameSettings.GetRandomGem(_container);
                gem.SetPosition(i, j);
                grid[i, j] = gem;
            }

            grid = ReplaceMatches(grid);
            var hasMove = CheckAvailableGame(grid);
            if (!hasMove)
            {
                Clear(grid);
                return Populate();
            }

            return grid;
        }

        private IGridPosition[,] ReplaceMatches(IGridPosition[,] grid)
        {
            for (var i = 0; i < _gameSettings.GridSettings.Horizontal; i++)
            for (var j = 0; j < _gameSettings.GridSettings.Vertical; j++)
            {
                var sequence = _sequenceChecker.CheckForSequence(grid, grid[i, j], out var list);
                if (sequence != SequenceType.None)
                {
                    grid[i, j] = ChangeElementAt(grid, i, j);
                    grid[i, j].SetPosition(i, j);
                }
            }

            return grid;
        }

        private IGridPosition ChangeElementAt(IGridPosition[,] grid, int x, int y)
        {
            var neighborsTypes = grid.GetNeighbors(x, y).Where(g => g != null).Select(n => (GemType) n.Id);
            var targetType = GetAvailableType(neighborsTypes);
            Destroy(grid[x, y].Transform.gameObject);
            
            return _gameSettings.GetGemOfType(_container, targetType);
        }

        public IEnumerator ProcessSequence(IGridPosition[,] grid, List<IGridPosition> sequence, SequenceType type)
        {
            //discovered that unity don't like interfaces in view components and bypass null checks
            if (sequence.Any(s => s == null || s.Equals(null)))
                yield break;

            var snapshot = sequence.Select(s => s.GetSnapshot()).ToList();
            foreach (var gridItem in sequence)
            {
                gridItem.Remove();
                grid[gridItem.X, gridItem.Y] = null;
            }

            OnSequence.Invoke(sequence.Count, sequence.Last().Transform.position);
            
            //delay for process sequences
            yield return new WaitForSeconds(DelayBetweenProcess);
            _repopulateGridRoutine = StartCoroutine(RepopulateGrid(grid, snapshot, type));
            yield return _repopulateGridRoutine;
        }

        public IEnumerator RepopulateGrid(IGridPosition[,] grid, List<GridPositionSnapshot> sequence, SequenceType type)
        {
            var ySize = grid.GetLength(1);
            switch (type)
            {
                case SequenceType.Horizontal:
                    for (var i = sequence.First().X; i <= sequence.Last().X ; i++)
                    {
                        for (var j = sequence.First().Y; j < ySize - 1; j++)
                        {
                            var firstItem = grid[i, j + 1];
                            var currentItem = grid[i, j];

                            grid[i, j] = firstItem;
                            grid[i, j + 1] = currentItem;
                            grid[i, j].SetPosition(grid[i, j].X, grid[i, j].Y - 1);
                        }
                        
                        grid[i, ySize - 1] = _gameSettings.GetRandomGem(_container);
                        grid[i, ySize - 1].SetPosition(i, ySize - 1);
                    }
                    break;
                
                case SequenceType.Vertical:
                    var sequenceHeight = 1 + (sequence.Last().Y - sequence.First().Y);
                    for (var i = sequence.First().Y + sequenceHeight; i <= ySize - 1; i++)
                    {
                        var lastItem = grid[sequence.First().X, i - sequenceHeight];
                        var currentItem = grid[sequence.First().X, i];

                        grid[sequence.First().X, i - sequenceHeight] = currentItem;
                        grid[sequence.First().X, i] = lastItem;
                    }

                    for (var i = 0; i < ySize - sequenceHeight; i++)
                        grid[sequence.First().X, i].SetPosition(sequence.First().X, i);

                    for (var i = 0; i < sequence.Count; i++)
                    {
                        grid[sequence.First().X, (ySize - 1) - i] = _gameSettings.GetRandomGem(_container);
                        grid[sequence.First().X, (ySize - 1) - i].SetPosition(sequence.First().X, (ySize - 1) - i);
                    }
                    break;
                
                default:
                    Debug.LogError($"Invalid State: {type}");
                    break;
            }

            grid = UpdateGrid(grid);
            _currentGrid = grid;
            yield return CheckForSequences(grid);
        }
        
        public IEnumerator CheckForSequences(IGridPosition[,] grid)
        {
            var allSequences = _sequenceChecker.CheckForSequence(grid);
            if (!allSequences.IsNullOrEmpty())
            {
                foreach (var keyValue in allSequences)
                {
                    var sequence = keyValue.Value;
                    if (sequence.IsNullOrEmpty())
                        continue;

                    var sequenceType = keyValue.Key;
                    var first = sequence.First();
                    yield return ProcessSequence(grid, first, sequenceType);
                }
            }
        }
        
        public IGridPosition[,] UpdateGrid(IGridPosition[,] grid)
        {
            var newGrid = new IGridPosition[grid.GetLength(0), grid.GetLength(1)];
            foreach (var gem in grid)
                newGrid[gem.X, gem.Y] = gem;

            return newGrid;
        }

        public IGridPosition[,] Clear(IGridPosition[,] grid)
        {
            StopAllCoroutines();
            
            if (grid.IsNullOrEmpty())
                return grid;
            
            var hSize = grid.GetLength(0);
            var vSize = grid.GetLength(1);

            foreach (var gemElement in grid)
            {
                if (gemElement != null && !gemElement.Equals(null))
                    Destroy(gemElement.Transform.gameObject);
            }

            for (int i = 0; i < _container.childCount; i++)
                Destroy(_container.GetChild(i).gameObject);
            
            return new IGridPosition[hSize, vSize];
        }

        private GemType GetAvailableType(IEnumerable<GemType> neighborTypes)
        {
            var values = Enum.GetValues(typeof(GemType)).Cast<GemType>().ToList();
            foreach (var neighborType in neighborTypes)
                values.Remove(neighborType);

            return values.GetRandom();
        }

        public bool CheckAvailableGame(IGridPosition[,] grid) => _sequenceChecker.CheckForNextMove(grid);
    }
}