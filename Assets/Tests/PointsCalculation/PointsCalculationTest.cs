using Controllers.Points;

namespace Tests.PointsCalculation
{
    public class PointsCalculationTest : Match3AssertMonoBehaviour
    {
        public void Awake()
        {
            //prepare
            Setup("[Points Calculation Test");
            var calculator = new PointsCalculator();
            
            //execute
            var result1 = calculator.Calculate(3, 3, 2, 3);
            //assert
            Assert(() => result1).ShouldBe(6).Because("We send the count and min count as 3").Run();
            
            //execute
            var result2 = calculator.Calculate(4, 3, 2, 3);
            //assert
            Assert(() => result2).ShouldBe(12).Because("1 more element multiplied by 3 should give 6 + 6").Run();
            
            //execute
            var result3 = calculator.Calculate(10, 2, 3, 4);
            //assert
            Assert(() => result3).ShouldBe(102)
                .Because("2 items multiplied by 3, and 8 bonus items multiplied by (3*4) gives 102").Run();
        }
    }
}
