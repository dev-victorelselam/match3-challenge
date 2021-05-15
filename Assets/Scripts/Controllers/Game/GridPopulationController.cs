using System;
using System.Collections.Generic;
using System.Linq;
using Controllers.Sequence;
using Domain;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controllers.Game
{
    public class GridPopulationController
    {
        private readonly Transform _container;
        private readonly GameSettings _gameSettings;
        private readonly ISequenceChecker _sequenceChecker;

        public GridPopulationController(Transform container, GameSettings gameSettings, ISequenceChecker sequenceChecker)
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

            return grid;
        }

        private IGridPosition[,] ReplaceMatches(IGridPosition[,] grid)
        {
            for (var i = 0; i < _gameSettings.GridSettings.Horizontal; i++)
            for (var j = 0; j < _gameSettings.GridSettings.Vertical; j++)
            {
                var list = _sequenceChecker.CheckForSequence(grid, grid[i, j]);
                if (!list.IsNullOrEmpty())
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
            Object.Destroy(grid[x, y].Transform.gameObject);
            
            return _gameSettings.GetGemOfType(_container, targetType);
        }

        public void Clear(IGridPosition[,] grid)
        {
            if (grid.IsNullOrEmpty())
                return;
            
            foreach (var gemElement in grid)
                Object.Destroy(gemElement.Transform.gameObject);
        }

        private GemType GetAvailableType(IEnumerable<GemType> neighborTypes)
        {
            var values = Enum.GetValues(typeof(GemType)).Cast<GemType>().ToList();
            foreach (var neighborType in neighborTypes)
                values.Remove(neighborType);

            return values.GetRandom();
        }
    }
}