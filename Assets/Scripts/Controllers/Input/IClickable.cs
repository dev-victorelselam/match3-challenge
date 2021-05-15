
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
        void MouseDown();
        
        /// <summary>
        /// Detected Mouse Up
        /// </summary>
        void MouseUp();
    }
}