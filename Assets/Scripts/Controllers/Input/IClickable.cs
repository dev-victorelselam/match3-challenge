
namespace Controllers.Input
{
    public interface IClickable
    {
        /// <summary>
        /// On element clicked by InputController
        /// </summary>
        void OnClick();
        
        /// <summary>
        /// Detected Mouse Down 
        /// </summary>
        void OnMouseDown();
        
        /// <summary>
        /// Detected Mouse Up
        /// </summary>
        void OnMouseUp();
    }
}