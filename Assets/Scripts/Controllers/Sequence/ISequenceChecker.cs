using System.Collections.Generic;
using Domain;

namespace Controllers.Sequence
{
    public interface ISequenceChecker
    {
        /// <summary>
        /// Check in all possible directions if this movement is valid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="elementsList"></param>
        /// <returns>If any valid sequence is found, return the sequence list, else return null</returns>
        SequenceType IsMovementValid(IGridPosition[,] grid, IGridPosition first, IGridPosition second, out List<IGridPosition> elementsList);

        /// <summary>
        /// Check for a sequence with the specified element in the grid
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="element"></param>
        /// <param name="sequenceList"></param>
        /// <returns>If any valid sequence is found, return the sequence list, else return null</returns>
        SequenceType CheckForSequence(IGridPosition[,] grid, IGridPosition element, out List<IGridPosition> sequenceList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        Dictionary<SequenceType, List<List<IGridPosition>>> CheckForSequence(IGridPosition[,] grid);
    }
}