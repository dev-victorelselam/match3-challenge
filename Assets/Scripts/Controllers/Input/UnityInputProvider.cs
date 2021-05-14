using UnityEngine;

namespace Controllers.Input
{
    public class UnityInputProvider : IInputProvider
    {
        public Vector2 MousePosition => UnityEngine.Input.mousePosition;
        
        public bool GetButtonDown(int buttonCode) => UnityEngine.Input.GetMouseButtonDown(0);

        public bool GetButtonUp(int buttonCode) => UnityEngine.Input.GetMouseButtonUp(0);
    }
}