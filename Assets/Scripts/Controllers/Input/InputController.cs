using System;
using UnityEngine;

namespace Controllers.Input
{
    public class InputController : MonoBehaviour, IInputController
    {
        public event Action<IClickable> OnClick;
        
        public float AllowedMovement = 8;
        
        private Vector3 _startMousePos;
        private bool _enabled;

        private IInputProvider _inputProvider;
        private IClickable _clickedObject;

        public void Awake()
        {
            _inputProvider = InputFactoring.CreateInstance();
            
            Enable(true);
            Update();
        }
    
        public void Enable(bool enable) => _enabled = enable;

        private void Update()
        {
            if (!_enabled)
                return;

            if (_inputProvider.GetButtonDown(0))
            {
                var origin = Camera.main.ScreenToWorldPoint(_inputProvider.MousePosition);
                var hit = Physics2D.Raycast(origin, Vector2.zero, 200);
                if (hit.collider != null &&
                    hit.collider.gameObject.GetComponent<IClickable>() != null)
                {
                    _startMousePos = _inputProvider.MousePosition;
                    _clickedObject = hit.collider.gameObject.GetComponent<IClickable>();
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
                
                _clickedObject = null;
            }
        }
    }
}