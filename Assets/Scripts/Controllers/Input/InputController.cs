using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Controllers.Input
{
    public class InputController : IInputController
    {
        public event Action<IClickable> OnClick;
        
        public float AllowedMovement = 8;
        
        private Vector3 _startMousePos;
        private bool _enabled;

        private readonly IInputProvider _inputProvider;
        private IClickable _clickedObject;

        public InputController()
        {
            _inputProvider = InputFactoring.CreateInstance();
            
            Enable(true);
            Update();
        }
    
        public void Enable(bool enable) => _enabled = enable;

        private async void Update()
        {
            while (true)
            {
                if (!_enabled)
                    await Task.Delay(100);

                if (_inputProvider.GetButtonDown(0))
                {
                    var origin = Camera.main.ScreenToWorldPoint(_inputProvider.MousePosition);
                    var hit = Physics2D.Raycast(origin, Vector2.zero, 200);
                    if (hit.collider != null &&
                        hit.collider.gameObject.GetComponent<IClickable>() != null)
                    {
                        _startMousePos = _inputProvider.MousePosition;

                        _clickedObject = hit.collider.gameObject.GetComponent<IClickable>();
                        _clickedObject.OnMouseDown();
                    }
                }

                if (_clickedObject != null && _inputProvider.GetButtonUp(0))
                {
                    var mouseDelta = Mathf.Abs(_inputProvider.MousePosition.magnitude - _startMousePos.magnitude);
                    if (mouseDelta < AllowedMovement)
                    {
                        _clickedObject.OnClick();
                        OnClick?.Invoke(_clickedObject);
                    }

                    _clickedObject.OnMouseUp();
                    _clickedObject = null;
                }
                
                await Task.Delay(10);
            }
        }
    }
}