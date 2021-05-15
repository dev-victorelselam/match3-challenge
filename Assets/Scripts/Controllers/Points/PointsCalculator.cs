using UnityEngine;

namespace Controllers.Points
{
    public class PointsCalculator : IPointsCalculator
    {
        public int Calculate(int elements, int minItemsCount, float pointsPerItem, float multiplier)
        {
            var defaultSequencePoints = minItemsCount * pointsPerItem;
            var extraPoints = (elements - minItemsCount) * (pointsPerItem * multiplier);

            //rounding up is fair
            return Mathf.CeilToInt(defaultSequencePoints + extraPoints); 
        }
    }
}