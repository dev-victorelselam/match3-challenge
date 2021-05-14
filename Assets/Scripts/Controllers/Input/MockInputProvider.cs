using UnityEngine;

namespace Controllers.Input
{
    public class MockInputProvider : IInputProvider
    {
        public Vector2 MousePosition => new Vector2(0, 0);
        
        public bool GetButtonDown(int buttonCode) => buttonCode == 0;
        public bool GetButtonUp(int buttonCode) => buttonCode == 0;
    }
}