using Controllers.Input;
using Domain;
using UnityEngine.Events;

namespace Controllers.Game
{
    public class GemSelectionController
    {
        public readonly UnityEvent<GemElement, GemElement> OnSelectionComplete = new UnityEvent<GemElement, GemElement>();
        public readonly UnityEvent<GemElement, GemElement> OnSelectionInvalid = new UnityEvent<GemElement, GemElement>();
        private readonly InputController _inputController;
        
        private GemElement _firstSelectedObject;
        private GemElement _secondSelectedObject;

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
            if (_firstSelectedObject)
            {
                //if same object, unselect and clean
                if (_firstSelectedObject == gemElement)
                {
                    gemElement.Unselect();
                    Clean();
                    return;
                }

                //if neighbor, proceed to complete selection
                if (_firstSelectedObject.IsNeighborFrom(gemElement))
                {
                    _secondSelectedObject = gemElement;
                    gemElement.Select();
                    
                    OnSelectionComplete.Invoke(_firstSelectedObject, _secondSelectedObject);
                    Clean();
                    return;
                }
               
                //if not neighbor, it is an invalid move
                _secondSelectedObject = gemElement;
                OnSelectionInvalid?.Invoke(_firstSelectedObject, _secondSelectedObject);
                Clean();
            }
            else
            {
                _firstSelectedObject = gemElement;
                gemElement.Select();
            }
        }

        private void Clean()
        {
            _firstSelectedObject = null;
            _secondSelectedObject = null;
        }
    }
}