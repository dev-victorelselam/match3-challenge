namespace Controllers.Points
{
    public interface IPointsCalculator
    {
        int Calculate(int elements, int minItemsCount, float pointsPerItem, float multiplier);
    }
}