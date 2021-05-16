using System.Collections.Generic;
using System.Linq;
using Controllers.Sequence;
using Domain;
using UnityEngine;

namespace Tests.SequenceChecker
{
    public class SequenceCheckerTest : Match3AssertMonoBehaviour
    {
        public class GridTestElement : IGridPosition
        {
            public GridTestElement(int x, int y, int id)
            {
               SetPosition(x, y);
               Id = id;
            }
            
            public int X { get; private set; }
            public int Y { get; private set;}
            public int Id { get; }
            public Transform Transform { get; }
            public void SetPosition(int x, int y)
            {
                X = x;
                Y = y;
            }

            public void Remove() { }
        }

        public void Awake()
        {
            //prepare
            Setup("[Grid Checker Test");
            var checker = Context.SequenceChecker;
            var hasSequenceGrid = GetSequenceGrid();
            var noSequenceGrid = GetNoSequenceGrid();
            
            //execute
            var result1 = checker.CheckForSequence(noSequenceGrid, noSequenceGrid[0, 0], out var sequence1);
            //assert
            Assert(() => result1).ShouldBe(SequenceType.None)
                .Because("That grid had no sequences").Run();
            
            //execute
            var result2 = checker.CheckForSequence(hasSequenceGrid, noSequenceGrid[0, 0], out var sequence2);
            var expectedSequence2 = new List<IGridPosition>
            {
                hasSequenceGrid[0, 0],
                hasSequenceGrid[1, 0],
                hasSequenceGrid[2, 0],
            };
            //assert
            Assert(() => result2).ShouldBe(SequenceType.Horizontal)
                .Because("First line has a sequence in it").Run();
            Assert(() => sequence2.First().Id).ShouldBe(expectedSequence2.First().Id)
                .Because("First line has a sequence of id 1 in it").Run();
        }

        private IGridPosition[,] GetNoSequenceGrid()
        {
            var grid = new IGridPosition[4, 4]; 
            grid[0, 0] = new GridTestElement(0, 0, 1);
            grid[1, 0] = new GridTestElement(1, 0, 2);
            grid[2, 0] = new GridTestElement(2, 0, 3);
            grid[3, 0] = new GridTestElement(3, 0, 4);
            
            grid[0, 1] = new GridTestElement(0, 1, 4);
            grid[1, 1] = new GridTestElement(1, 1, 3);
            grid[2, 1] = new GridTestElement(2, 1, 2);
            grid[3, 1] = new GridTestElement(3, 1, 1);
            
            grid[0, 2] = new GridTestElement(0, 2, 1);
            grid[1, 2] = new GridTestElement(1, 2, 2);
            grid[2, 2] = new GridTestElement(2, 2, 3);
            grid[3, 2] = new GridTestElement(3, 2, 4);
            
            grid[0, 3] = new GridTestElement(0, 3, 1);
            grid[1, 3] = new GridTestElement(1, 3, 2);
            grid[2, 3] = new GridTestElement(2, 3, 3);
            grid[3, 3] = new GridTestElement(3, 3, 1);
            
            return grid;
        }
        
        private IGridPosition[,] GetSequenceGrid()
        {
            var grid = new IGridPosition[4, 4]; 
            grid[0, 0] = new GridTestElement(0, 0, 1);
            grid[1, 0] = new GridTestElement(1, 0, 1);
            grid[2, 0] = new GridTestElement(2, 0, 1);
            grid[3, 0] = new GridTestElement(3, 0, 2);
            
            grid[0, 1] = new GridTestElement(0, 1, 1);
            grid[1, 1] = new GridTestElement(1, 1, 3);
            grid[2, 1] = new GridTestElement(2, 1, 1);
            grid[3, 1] = new GridTestElement(3, 1, 1);
            
            grid[0, 2] = new GridTestElement(0, 2, 1);
            grid[1, 2] = new GridTestElement(1, 2, 4);
            grid[2, 2] = new GridTestElement(2, 2, 1);
            grid[3, 2] = new GridTestElement(3, 2, 1);
            
            grid[0, 3] = new GridTestElement(0, 3, 1);
            grid[1, 3] = new GridTestElement(1, 3, 2);
            grid[2, 3] = new GridTestElement(2, 3, 3);
            grid[3, 3] = new GridTestElement(3, 3, 1);
            
            return grid;
        }
    }
}
