using UnityEngine;

namespace Domain
{
    public class GridPositionSnapshot
    {
        public Vector3 Position;
        public Vector3 EulerAngles;

        public int X;
        public int Y;
        
        public GridPositionSnapshot(IGridPosition gridElement)
        {
            Position = gridElement.Transform.position;
            EulerAngles = gridElement.Transform.eulerAngles;

            X = gridElement.X;
            Y = gridElement.Y;
        }
    }
}