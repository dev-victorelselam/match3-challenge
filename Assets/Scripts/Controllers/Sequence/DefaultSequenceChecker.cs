using System.Collections.Generic;
using System.Linq;
using Domain;
using UnityEngine;

namespace Controllers.Sequence
{
    public class DefaultSequenceChecker : ISequenceChecker
    {
        private readonly int _streakCount;

        public DefaultSequenceChecker(int minSequenceCount)
        {
            _streakCount = minSequenceCount;
        }
        
        public SequenceType IsMovementValid(IGridPosition[,] grid, IGridPosition first, IGridPosition second, out List<IGridPosition> elementsList)
        {
            var firstValid = CheckForSequence(grid, first, out var firstList);
            var secondValid = CheckForSequence(grid, second, out var secondList);

            if (firstValid != SequenceType.None)
            {
                elementsList = firstList;
                return firstValid;
            }

            if (secondValid != SequenceType.None)
            {
                elementsList = secondList;
                return secondValid;
            }

            elementsList = null;
            return SequenceType.None;
        }

        public SequenceType CheckForSequence(IGridPosition[,] grid, IGridPosition element, out List<IGridPosition> sequenceList)
        {
            //since 1 element can't trigger horizontal and vertical at same time, 
            //I decided to give preference to horizontal.
            
            var horizontal = CheckHorizontal(grid, element);
            if (horizontal.Count >= _streakCount)
            {
                sequenceList = horizontal;
                return SequenceType.Horizontal;
            }

            var vertical = CheckVertical(grid, element);
            if (vertical.Count >= _streakCount)
            {
                sequenceList = vertical;
                return SequenceType.Vertical;
            }

            sequenceList = null;
            return SequenceType.None;
        }

        public Dictionary<SequenceType, List<List<IGridPosition>>> CheckForSequence(IGridPosition[,] grid)
        {
            var sequenceByType = new Dictionary<SequenceType, List<List<IGridPosition>>>();
            
            sequenceByType[SequenceType.Horizontal] = new List<List<IGridPosition>>();
            for (var column = 0; column < grid.GetLength(1); column++)
            {
                var hList = CheckHorizontal(grid, column);
                if (hList.Count >= _streakCount)
                    sequenceByType[SequenceType.Horizontal].Add(hList);
            }
            
            sequenceByType[SequenceType.Vertical] = new List<List<IGridPosition>>();
            for (var line = 0; line < grid.GetLength(0); line++)
            {
                var vList = CheckVertical(grid, line);
                if (vList.Count >= _streakCount)
                    sequenceByType[SequenceType.Vertical].Add(vList);
            }

            return sequenceByType;
        }

        private List<IGridPosition> CheckHorizontal(IGridPosition[,] grid, IGridPosition element)
        {
            foreach (var gridPosition in grid)
            {
                if (gridPosition == null)
                    return new List<IGridPosition>();
            }
            
            var id = element.Id;
            var list = new List<IGridPosition>();
            var validList = new List<IGridPosition>();
            
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var value = grid[i, element.Y];
                if (value.Id == id)
                {
                    list.Add(value);
                    if (list.Count >= _streakCount)
                        validList = list.ToList();
                }
                    
                else
                    list.Clear();
            }

            return validList;
        }

        private List<IGridPosition> CheckHorizontal(IGridPosition[,] grid, int column, int streakCount = 0)
        {
            foreach (var gridPosition in grid)
            {
                if (gridPosition == null)
                    return new List<IGridPosition>();
            }
            
            if (streakCount == 0)
                streakCount = _streakCount;
            
            IGridPosition lastValue = null;
            var list = new List<IGridPosition>();
            var validList = new List<IGridPosition>();
            
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var value = grid[i, column];
                if (lastValue == null)
                {
                    AddValue(value);
                    continue;
                }
                    
                if (value.Id == lastValue.Id)
                {
                    AddValue(value);
                    if (list.Count >= streakCount)
                        validList = list.ToList();
                }

                else
                {
                    list.Clear();
                    AddValue(value);
                }
            }

            return validList;

            void AddValue(IGridPosition value)
            {
                list.Add(value);
                lastValue = value;
            }
        }
            
        private List<IGridPosition> CheckVertical(IGridPosition[,] grid, IGridPosition element)
        {
            foreach (var gridPosition in grid)
            {
                if (gridPosition == null)
                    return new List<IGridPosition>();
            }
            
            var id = element.Id;
            var list = new List<IGridPosition>();
            var validList = new List<IGridPosition>();
            
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                var value = grid[element.X, i];
                if (value.Id == id)
                {
                    list.Add(value);
                    if (list.Count >= _streakCount)
                        validList = list.ToList();
                }
                    
                else
                    list.Clear();
            }

            return validList;
        }
        
        private List<IGridPosition> CheckVertical(IGridPosition[,] grid, int line, int streakCount = 0)
        {
            foreach (var gridPosition in grid)
            {
                if (gridPosition == null)
                    return new List<IGridPosition>();
            }
            
            if (streakCount == 0)
                streakCount = _streakCount;
            
            IGridPosition lastValue = null;
            var list = new List<IGridPosition>();
            var validList = new List<IGridPosition>();
            
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                var value = grid[line, i];
                if (lastValue == null)
                {
                    AddValue(value);
                    continue;
                }
                    
                if (value.Id == lastValue.Id)
                {
                    AddValue(value);
                    if (list.Count >= streakCount)
                        validList = list.ToList();
                }

                else
                {
                    list.Clear();
                    AddValue(value);
                }
            }

            return validList;

            void AddValue(IGridPosition value)
            {
                list.Add(value);
                lastValue = value;
            }
        }

        //this is not efficient but I couldn't find the deterministic algorithm. 
        public bool CheckForNextMove(IGridPosition[,] grid, out IGridPosition availableElement)
        {
            //check horizontal require column length
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                var h = CheckHorizontal(grid, i, 2);
                for (var j = 0; j < h.Count; j++)
                {
                    var gridItem = h[j];
                    var neighbors = grid.GetDiagonalNeighbors(gridItem.X, gridItem.Y);
                    IGridPosition first;
                    IGridPosition second;
                    IGridPosition sameLine;
                    //if first, we want left diagonals
                    if (j == 0)
                    {
                        first = neighbors[0];
                        second = neighbors[1];
                        sameLine = grid.SafeGetAt(gridItem.X - 2, gridItem.Y);
                    }
                    //if second, we want right diagonals
                    else
                    {
                        first = neighbors[2];
                        second = neighbors[3];
                        sameLine = grid.SafeGetAt(gridItem.X + 2, gridItem.Y);
                    }

                    if (first != null && !h.Contains(first) && first.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y: {gridItem.Y} in up!");
                        availableElement = first;
                        return true;
                    }

                    if (second != null && !h.Contains(second) && second.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y: {gridItem.Y} in down!");
                        availableElement = second;
                        return true;
                    }
                    
                    if (sameLine != null && !h.Contains(sameLine) && sameLine.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y: {gridItem.Y} in same line!");
                        availableElement = sameLine;
                        return true;
                    }
                }
            }
            
            //check vertical require line length
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var h = CheckVertical(grid, i, 2);
                for (var j = 0; j < h.Count; j++)
                {
                    var gridItem = h[j];
                    var neighbors = grid.GetDiagonalNeighbors(gridItem.X, gridItem.Y);
                    IGridPosition first;
                    IGridPosition second;
                    IGridPosition sameColumn;
                    //if first, we want down diagonals
                    if (j == 0)
                    {
                        first = neighbors[1];
                        second = neighbors[3];
                        sameColumn = grid.SafeGetAt(gridItem.X, gridItem.Y - 2);
                    }
                    //if second, we want up diagonals
                    else
                    {
                        first = neighbors[0];
                        second = neighbors[2];
                        sameColumn = grid.SafeGetAt(gridItem.X, gridItem.Y + 2);
                    }

                    if (first != null && !h.Contains(first) && first.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y:{gridItem.Y} in left!");
                        availableElement = first;
                        return true;
                    }

                    if (second != null && !h.Contains(second) && second.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y:{gridItem.Y} in right!");
                        availableElement = second;
                        return true;
                    }
                    
                    if (sameColumn != null && !h.Contains(sameColumn) && sameColumn.Id == gridItem.Id)
                    {
                        Debug.Log($"Available game at: X:{gridItem.X} Y: {gridItem.Y} in same line!");
                        availableElement = sameColumn;
                        return true;
                    }
                }
            }

            Debug.Log("No available game!");
            availableElement = null;
            return false;
        }
    }
}