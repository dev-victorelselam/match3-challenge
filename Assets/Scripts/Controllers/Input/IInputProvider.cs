using UnityEngine;

namespace Controllers.Input
{
    public interface IInputProvider
    {
        Vector2 MousePosition { get; }
        
        bool GetButtonDown(int buttonCode);
        bool GetButtonUp(int buttonCode);
    }
}