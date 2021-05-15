using UnityEngine;

namespace Domain
{
    public interface IGridPosition
    {
        int X { get; }
        int Y { get; }
        int Id { get; }
        
        Transform Transform { get; }

        void SetPosition(int x, int y);
        void Remove();
    }
}