using System.Collections.Generic;
using Domain;

namespace Controllers.Sequence
{
    public class DefaultSequenceChecker : ISequenceChecker
    {
        private readonly int _streakCount;

        public DefaultSequenceChecker(int minSequenceCount)
        {
            _streakCount = minSequenceCount;
        }
        
        public bool IsMovementValid(IGridPosition[,] grid, IGridPosition first, IGridPosition second, out List<IGridPosition> elementsList)
        {
            var firstList = CheckForSequence(grid, first);
            var secondList = CheckForSequence(grid, second);

            if (firstList != null)
            {
                elementsList = firstList;
                return true;
            }

            if (secondList != null)
            {
                elementsList = secondList;
                return true;
            }

            elementsList = null;
            return false;
        }

        public List<IGridPosition> CheckForSequence(IGridPosition[,] grid, IGridPosition element)
        {
            //since 1 element can't trigger horizontal and vertical at same time, 
            //I decided to give preference to horizontal.
            
            var horizontal = CheckHorizontal(grid, element);
            if (horizontal.Count >= _streakCount)
                return horizontal;
            
            var vertical = CheckVertical(grid, element);
            if (vertical.Count >= _streakCount)
                return vertical;

            return null;
        }
        
        private List<IGridPosition> CheckHorizontal(IGridPosition[,] grid, IGridPosition element)
        {
            var id = element.Id;
            var list = new List<IGridPosition>();
            for (var i = 0; i < grid.GetLength(0); i++)
            {
                var value = grid[i, element.Y];
                if (value.Id == id)
                {
                    list.Add(value);
                    if (list.Count >= _streakCount)
                        return list;
                }
                    
                else
                    list.Clear();
            }

            return list;
        }
            
        private List<IGridPosition> CheckVertical(IGridPosition[,] grid, IGridPosition element)
        {
            var id = element.Id;
            var list = new List<IGridPosition>();
            for (var i = 0; i < grid.GetLength(1); i++)
            {
                var value = grid[element.X, i];
                if (value.Id == id)
                {
                    list.Add(value);
                    if (list.Count >= _streakCount)
                        return list;
                }
                    
                else
                    list.Clear();
            }

            return list;
        }
    }
}