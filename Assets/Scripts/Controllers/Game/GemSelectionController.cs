using Context;
using Controllers.Input;
using Domain;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class GemSelectionController
    {
        public UnityEvent<GemElement, GemElement> OnSelectionComplete = new UnityEvent<GemElement, GemElement>();
        private readonly InputController _inputController;
        
        private GemElement _firstClickedObject;
        private GemElement _secondClickedObject;

        public GemSelectionController(InputController inputController)
        {
            _inputController = inputController;
            _inputController.OnClick += OnClick;
        }

        ~GemSelectionController()
        {
            _inputController.OnClick -= OnClick;
        }

        private void OnClick(IClickable clickable)
        {
            if (clickable is GemElement gemElement)
                AssignGem(gemElement);
        }

        private void AssignGem(GemElement gemElement)
        {
            if (_firstClickedObject)
            {
                if (_firstClickedObject == gemElement)
                {
                    _firstClickedObject = null;
                    gemElement.Unselect();
                    return;
                }

                if (_firstClickedObject.IsNeighbor(gemElement))
                {
                    _secondClickedObject = gemElement;
                    gemElement.Select();
                }
            }
            else
            {
                _firstClickedObject = gemElement;
                gemElement.Select();
            }
        }
    }
}