using System;

namespace Controllers.Input
{
    public interface IInputController
    {
        event Action<IClickable> OnClick;
        void Enable(bool enable);
    }
}