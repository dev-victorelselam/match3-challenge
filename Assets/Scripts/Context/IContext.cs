using Controllers.Game;
using Controllers.Input;
using Controllers.Sequence;
using Domain;

namespace Context
{
    public interface IContext
    {
        Environment Environment { get; }
        GameSettings GameSettings { get; }
        GameController GameController { get; }
        InputController InputController { get; }
        ISequenceChecker SequenceChecker { get; }
    }
}